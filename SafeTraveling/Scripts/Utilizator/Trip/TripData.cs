using System;
using System.Collections.Generic;

namespace SafeTraveling
{
	public class TripData
	{
		public string DestinatiePrincipala;
		public string TipulParticipantilor;
		public string DataPlecare;
		public string DataIntoarcere;
		public string LocatiePlecare;
		public string OraPlecare;
		public string Observatii;
		public string TripId;
		public List<string> TripDataArray = new List<string>();

		public TripData (string[] ArrayData)
		{
			DestinatiePrincipala = ArrayData[0];
			TipulParticipantilor = ArrayData[1];
			DataPlecare = ArrayData[2];
			DataIntoarcere = ArrayData[3];
			LocatiePlecare = ArrayData[4];
			OraPlecare = ArrayData[5];
			Observatii = ArrayData[6];
			TripId = ArrayData [7];

			TripDataArray.Add (DestinatiePrincipala);
			TripDataArray.Add (TipulParticipantilor);
			TripDataArray.Add (DataPlecare);
			TripDataArray.Add (DataIntoarcere);
			TripDataArray.Add (LocatiePlecare);
			TripDataArray.Add (OraPlecare);
			TripDataArray.Add (Observatii);
			TripDataArray.Add (TripId);
		}
	}
}

