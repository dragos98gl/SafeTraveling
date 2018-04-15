using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices.Tasks
{
    class EditUserInfo
    {
        public static void Handle(string[] Messages,List<TripClient> ClientsList)
        {
            string Camp = Messages[1];
            string NumarTelefon = Messages[2];
            string NewValue = Messages[3];

            foreach(TripClient tClient in ClientsList)
                if (tClient.TClientData.NumarTelefon.Equals(NumarTelefon))
                {
                    using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                    {
                        Database.Open();

                        string Command = string.Empty;

                        switch (Camp)
                        {
                            case "nume":
                                {
                                    Command = "UPDATE Utilizatori SET [Nume] ='" + NewValue + "' WHERE [NumarTelefon]='" + NumarTelefon + "'";
                                } break;
                            case "prenume":
                                {
                                    Command = "UPDATE Utilizatori SET [Prenume] ='" + NewValue + "' WHERE [NumarTelefon]='" + NumarTelefon + "'";
                                } break;
                            case "varsta":
                                {
                                    Command = "UPDATE Utilizatori SET [Varsta] ='" + NewValue + "' WHERE [NumarTelefon]='" + NumarTelefon + "'";
                                } break;
                            case "sex":
                                {
                                    Command = "UPDATE Utilizatori SET [Sex] ='" + NewValue + "' WHERE [NumarTelefon]='" + NumarTelefon + "'";
                                } break;
                            case "e-mail":
                                {
                                    Command = "UPDATE Utilizatori SET [Email] ='" + NewValue + "' WHERE [NumarTelefon]='" + NumarTelefon + "'";
                                } break;
                        }

                        if (!Command.Equals(string.Empty))
                        {
                            SqlCommand cmd = new SqlCommand(Command, Database);
                            cmd.ExecuteNonQuery();

                            tClient.OUTPUT_SEND(new string[] { _Details.EditUserInfo, (1).ToString() });
                        }
                        else
                            tClient.OUTPUT_SEND(new string[] { _Details.EditUserInfo, (0).ToString() });
                    }
                }
        }
    }
}
