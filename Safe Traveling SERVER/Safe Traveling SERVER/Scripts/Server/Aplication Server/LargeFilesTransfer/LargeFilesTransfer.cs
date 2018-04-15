using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LargeFilesTransfer.Tasks;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices.QuestionPool;
using Safe_Traveling_SERVER.Scripts.Visual_Scripts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LargeFilesTransfer
{
    class LargeFilesTransfer
    {
        const int SleepTime = 300;
        TcpListener host;
        List<Excursie> Excursii;

        public void Start(TcpListener host,List<Excursie> Excursii)
        {
            this.host = host;
            this.Excursii = Excursii;

            new Thread(new ThreadStart(LoadClients)).Start();
        }

        private void LoadClients()
        {
            while (true)
            {
                if (host.Pending())
                    new Thread(new ParameterizedThreadStart(SendOrUpload)).Start(host.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void SendOrUpload(object newClient)
        {
            TcpClient Client = (TcpClient)newClient;
            NetworkStream ns = Client.GetStream();

            string[] Tag = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns));

            switch (Tag[0])
            {
                case _Details.GetGalleryPhotoByIndex:
                    {
                        string IdExcursie = Tag[1];
                        int PhotoIndex = int.Parse(Tag[2]);

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [URL] FROM Excursii WHERE [UniqueId]='" + IdExcursie + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                            {
                                string[] PhotosPath = Directory.GetFiles(Reader[0] as string + @"\Galerie");

                                Reader.Close();

                                using (FileStream fs = File.Open(PhotosPath[PhotoIndex], FileMode.Open))
                                {
                                    int PackSize = 1000;
                                    int TotalLength = (int)fs.Length;
                                    int NoOfPackets = (int)Math.Ceiling((double)fs.Length / (double)PackSize);
                                    int CurrentPackSize;

                                    for (int i = 0; i < NoOfPackets; i++)
                                    {
                                        if (TotalLength > PackSize)
                                        {
                                            CurrentPackSize = PackSize;
                                            TotalLength -= CurrentPackSize;
                                        }
                                        else
                                            CurrentPackSize = TotalLength;

                                        byte[] CurrentBytes = new byte[CurrentPackSize];
                                        int ReadedLength = fs.Read(CurrentBytes, 0, CurrentBytes.Length);

                                        ns.Write(CurrentBytes, 0, ReadedLength);
                                    }
                                }
                            }
                        }

                        Client.Close();
                    } break;
                case _Details.GetGalleryCount:
                    {
                        string IdExcursie = Tag[1];

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [URL] FROM Excursii WHERE [UniqueId]='" + IdExcursie + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                            {
                                string[] PhotosPath = Directory.GetFiles(Reader[0] as string + @"\Galerie");
                                _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { PhotosPath.Length.ToString() }));

                                Reader.Close();
                            }
                        }

                        Client.Close();
                    } break;
                case _Details.AddPhotoToGallery:
                    {
                        string IdExcursie = Tag[1];
                        MemoryStream ms = new MemoryStream();
                        byte[] Buffer = new byte[1000];

                        int ReadedBytes;
                        while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
                        {
                            ms.Write(Buffer, 0, ReadedBytes);
                        }

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [URL] FROM Excursii WHERE [UniqueId]='" + IdExcursie + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                            {
                                Image i = Image.FromStream(ms);
                                Guid newGuid = Guid.NewGuid();
                                i.Save((string)Reader[0] + @"\Galerie\" + newGuid.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

                                Reader.Close();
                            }
                        }

                        Client.Close();
                    } break;
                case _Details.UpdateProfilePic:
                    {
                        string NrTel = Tag[1];
                        MemoryStream ms = new MemoryStream();
                        byte[] Buffer = new byte[1000];

                        int ReadedBytes;
                        while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
                        {
                            ms.Write(Buffer, 0, ReadedBytes);
                        }

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [URL] FROM Utilizatori WHERE [NumarTelefon]='" + NrTel + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                            {
                                Image i = Image.FromStream(ms);
                                i.Save((string)Reader[0] + @"\Profile.png", System.Drawing.Imaging.ImageFormat.Png);

                                string LowSizePhoto = CreateLowSizePhoto.GetLowSizePhotoBase64((string)Reader[0] + @"\Profile.png");

                                Reader.Close();

                                cmd = new SqlCommand("UPDATE Utilizatori SET [LowSizePhoto]='" + LowSizePhoto + "' WHERE [NumarTelefon]='" + NrTel + "'", Database);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        Client.Close();
                    } break;
                case _Details.GetProfilePic:
                    {
                        string NrTel = Tag[1];

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            string t = "SELECT * FROM Utilizatori WHERE [NumarTelefon]='" + NrTel + "'";

                            SqlCommand cmd = new SqlCommand(t, Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                            {
                                string UserLowSizePhoto = (string)Reader[8];

                                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(UserLowSizePhoto)))
                                {
                                    int PackSize = 1000;
                                    int TotalLength = (int)ms.Length;
                                    int NoOfPackets = (int)Math.Ceiling((double)ms.Length / (double)PackSize);
                                    int CurrentPackSize;

                                    for (int i = 0; i < NoOfPackets; i++)
                                    {
                                        if (TotalLength > PackSize)
                                        {
                                            CurrentPackSize = PackSize;
                                            TotalLength -= CurrentPackSize;
                                        }
                                        else
                                            CurrentPackSize = TotalLength;

                                        byte[] CurrentBytes = new byte[CurrentPackSize];
                                        int ReadedLength = ms.Read(CurrentBytes, 0, CurrentBytes.Length);

                                        ns.Write(CurrentBytes, 0, ReadedLength);
                                    }
                                }
                            }

                            Client.Close();
                        }
                    } break;

                case _Details.GetTripId:
                    {
                        string NrTel = Tag[1];
                        string TripId = "error";

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [UniqueId] FROM Excursii WHERE [Numar Telefon]='" + NrTel + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                                TripId = (string)Reader[0];

                            _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { TripId }));

                            ns.Dispose();
                            Client.Close();
                        }
                    } break;
                case _Details.GetUserName:
                    {
                        string NrTel = Tag[1];
                        string NumeUtilizator = "error";

                        using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database.Open();

                            SqlCommand cmd = new SqlCommand("SELECT [Nume],[Prenume] FROM Utilizatori WHERE [NumarTelefon]='" + NrTel + "'", Database);
                            SqlDataReader Reader = cmd.ExecuteReader();

                            if (Reader.Read())
                                NumeUtilizator = (string)Reader[0] + " " + (string)Reader[1];

                            _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { NumeUtilizator }));

                            ns.Dispose();
                            Client.Close();
                        }
                    } break;
                case _Details.NewQuestionPool:
                    {
                        string NrTel = Tag[1];
                        string TripId = Tag[2];

                        foreach (Excursie e in Excursii)
                            if (TripId.Equals(e.UniqueId))
                                e.QuestionPools.Add(new QuestionPool(e, Tag));
                    } break;
                case _Details.VoteQuestionPool:
                    {
                        string TripId = Tag[1];
                        int index = int.Parse(Tag[2]);
                        string Id = Tag[3];

                        foreach (Excursie e in Excursii)
                            if (TripId.Equals(e.UniqueId))
                                foreach (QuestionPool q in e.QuestionPools)
                                    if (q.Id.Equals(Id))
                                        q.Vote(index);

                        ns.Dispose();
                        Client.Close();
                    } break;
                case _Details.RequestVotesQuestionPool:
                    {
                        string TripId = Tag[1];

                        string[] Intrebari = null;
                        string[] Ids = null;

                        foreach (Excursie e in Excursii)
                            if (TripId.Equals(e.UniqueId))
                            {
                                Intrebari = new string[e.QuestionPools.Count];
                                Ids = new string[e.QuestionPools.Count];

                                for (int i = 0; i < Intrebari.Length; i++)
                                {
                                    Intrebari[i] = e.QuestionPools[i].Intrebare;
                                    Ids[i] = e.QuestionPools[i].Id;
                                }
                            }

                        _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { string.Join(",", Intrebari), string.Join(",", Ids) }));

                        //string Id = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns))[0];

                        foreach (Excursie e in Excursii)
                            if (TripId.Equals(e.UniqueId))
                                foreach (QuestionPool q in e.QuestionPools)
                                {
                                    string[] Votes = new string[q.Variante.Length];

                                    for (int i = 0; i < q.Variante.Length; i++)
                                        Votes[i] = q.Variante[i] + "," + q.GetPercentageOf(i);

                                    _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(Votes));
                                }

                        ns.Dispose();
                        Client.Close();
                    } break;
                case _Details.BeSmartSimpleQuery:
                    {
                        HandleBeSmartClient.Handle(Tag,ns);

                        ns.Dispose();
                        Client.Close();
                    } break;
            }
        }
    }
}