using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses {
	public static class PropertyTools {







		// Method to determine GEI Survey Language Type
		public static int GetGEISurveyLanguageIndex(GCCPropertyShortCode shortCode)
		{
			switch (shortCode)
			{
				case GCCPropertyShortCode.GCC:
					return 1;
				case GCCPropertyShortCode.RR:
					return 1;
				case GCCPropertyShortCode.HRCV:
					return 1;
				case GCCPropertyShortCode.FD:
					return 1;
				case GCCPropertyShortCode.HA:
					return 1;
				case GCCPropertyShortCode.ECV:
					return 1;
				case GCCPropertyShortCode.NAN:
					return 1;
				case GCCPropertyShortCode.CCH:
					return 1;
				case GCCPropertyShortCode.CMR:
					return 1;
				case GCCPropertyShortCode.CDC:
					return 1;
				case GCCPropertyShortCode.CNSH:
					return 1;
				case GCCPropertyShortCode.CNSS:
					return 1;
				case GCCPropertyShortCode.GAG:
					return 1;
				case GCCPropertyShortCode.ECS:
					return 1;
				case GCCPropertyShortCode.FL:
					return 1;
				case GCCPropertyShortCode.GD:
					return 1;
				case GCCPropertyShortCode.SSKD:
					return 1;
				case GCCPropertyShortCode.SCTI:
					return 0;
				case GCCPropertyShortCode.CNB:
					return 0;
				case GCCPropertyShortCode.SCBE:
					return 1;
				case GCCPropertyShortCode.BSQ:
					return 1;
				case GCCPropertyShortCode.WDB:
					return 0;
				case GCCPropertyShortCode.AJA:
					return 1;
				case GCCPropertyShortCode.GBH:
					return 1;

				case GCCPropertyShortCode.ECB:
					return 1;

				case GCCPropertyShortCode.ECF:
					return 1;

				case GCCPropertyShortCode.ECGR:
					return 1;

				case GCCPropertyShortCode.ECM:
					return 1;

				default:
					return 1;
			}
		}


		/// <summary>
		/// Gets the home URL for a particular Casino.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		public static string GetCasinoURL( GCCPropertyShortCode shortCode ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.GCC:
					return "http://gcgaming.com/";
				case GCCPropertyShortCode.RR:
					return "http://www.riverrock.com/";
				case GCCPropertyShortCode.HRCV:
					return "http://www.hardrockcasinovancouver.com/";
				case GCCPropertyShortCode.FD:
					return "http://www.fraserdowns.com/";
				case GCCPropertyShortCode.HA:
					return "http://www.hastingsracecourse.com/";
				case GCCPropertyShortCode.ECV:
					return "http://www.viewroyalcasino.com/";
				case GCCPropertyShortCode.NAN:
					return "http://www.casinonanaimo.com/";
				case GCCPropertyShortCode.CCH:
					return "http://chanceschilliwack.com/";
				case GCCPropertyShortCode.CMR:
					return "http://chancesmapleridge.com/";
				case GCCPropertyShortCode.CDC:
					return "http://chancesdawsoncreek.com/";
				case GCCPropertyShortCode.CNSH:
					return "http://www.casinonovascotia.com/";
				case GCCPropertyShortCode.CNSS:
					return "http://sydney.casinonovascotia.com/";
				case GCCPropertyShortCode.GAG:
					return "http://www.greatamericancasino.com/";
				case GCCPropertyShortCode.ECS:
					return "http://www.elementscasino.com/";
				case GCCPropertyShortCode.FL:
					return "http://www.flamborodowns.com/";
				case GCCPropertyShortCode.GD:
					return "http://www.georgiandowns.com/";
				case GCCPropertyShortCode.SSKD:
					return "http://shorelinescasinos.com/";
				case GCCPropertyShortCode.SCTI:
					return "http://shorelinescasinos.com/";
				case GCCPropertyShortCode.CNB:
					return "http://www.casinonb.ca/";
				case GCCPropertyShortCode.SCBE:
					return "http://shorelinescasinos.com/belleville/";

				case GCCPropertyShortCode.BSQ:
					return "http://www.bingoesquimalt.ca/";


				case GCCPropertyShortCode.WDB:
					return "http://casinowoodbine.com/";

					

				case GCCPropertyShortCode.AJA:
					return "https://casinoajax.com/";


				case GCCPropertyShortCode.GBH:
					return "http://gbhcasino.com/";


				case GCCPropertyShortCode.ECB:
					return "http://elementscasinobrantford.com/";


				case GCCPropertyShortCode.ECF:
					return "http://elementscasinoflamboro.com/";


				case GCCPropertyShortCode.ECGR:
					return "http://elementscasinograndriver.com/";


				case GCCPropertyShortCode.ECM:
					return "http://elementscasinomohawk.com/";



				default:
					return String.Empty;
			}
		}

		/// <summary>
		/// Gets the phone number for a particular Casino.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		/// <param name="gagLocation"></param>
		public static string GetPhoneNumber( GCCPropertyShortCode shortCode, int gagLocation ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.GCC:
					return "";
				case GCCPropertyShortCode.RR:
					return "604.273.1895";
				case GCCPropertyShortCode.HRCV:
					return "604.523.6888";
				case GCCPropertyShortCode.ECS:
				case GCCPropertyShortCode.FD:
					return "604.576.9141";
				case GCCPropertyShortCode.HA:
					return "604.254.1631";
				case GCCPropertyShortCode.ECV:
					return "250.391.0311";
				case GCCPropertyShortCode.NAN:
					return "250.753.3033";
				case GCCPropertyShortCode.CCH:
					return "604.701.3800";
				case GCCPropertyShortCode.CMR:
					return "604.751.5688";
				case GCCPropertyShortCode.CDC:
					return "250.782.7752";
				case GCCPropertyShortCode.CNSH:
					return "902.425.7777";
				case GCCPropertyShortCode.CNSS:
					return "902.563-7777";
				case GCCPropertyShortCode.GAG:
					switch ( gagLocation ) {
						case 1: //Everett
							return "425.347.1669";
						case 2: //Lakewood
							return "253.396.0500";
						case 3: //Tukwila
							return "206.244.5400";
						case 4: //DeMoines
							return "555.555.5555";
					}
					break;
				case GCCPropertyShortCode.FL:
					return String.Empty;
				case GCCPropertyShortCode.GD:
					return String.Empty;
				case GCCPropertyShortCode.SSKD:
					return "705.939.2400";
				case GCCPropertyShortCode.SCTI:
					return "613.382.6800";
				case GCCPropertyShortCode.CNB:
					return "877-859-7775";
				case GCCPropertyShortCode.SCBE:
					return string.Empty;
				case GCCPropertyShortCode.BSQ:
					return string.Empty;


				case GCCPropertyShortCode.GBH:
					return "888-294-3766";


				case GCCPropertyShortCode.WDB:
					return "888-345-7568";


				case GCCPropertyShortCode.AJA:
					return "866-445-3939";


				case GCCPropertyShortCode.ECB:
					return "1-888-694-6946";


				case GCCPropertyShortCode.ECF:
					return "905-628-4275";


				case GCCPropertyShortCode.ECGR:
					return "516-846-2022";


				case GCCPropertyShortCode.ECM:
					return "605-854-4053";


			}
			return String.Empty;
		}

		/// <summary>
		/// Gets the show lounge name for a particular Casino.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		/// <param name="hrcvOrECLocation">If 1, returns Asylum for HRCV. If 2, Returns UnListed for HRCV. 0 or anything else returns both.</param>
		public static string GetShowLoungeName(GCCPropertyShortCode shortCode, int hrcvOrECLocation = 0 ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.RR:
					//return "Lulu's Lounge";
					return "Curve Lounge";
				case GCCPropertyShortCode.HRCV:
					if ( hrcvOrECLocation != 1 && hrcvOrECLocation != 2 ) {
						return "Asylum Gastro-Pub and Live Sound Stage or UnListed Buffet and Lounge";
					} else if ( hrcvOrECLocation == 1 ) {
						return "Asylum Gastro-Pub and Live Sound Stage";
					} else {
						return "UnListed Buffet and Lounge";
					}
				case GCCPropertyShortCode.ECS:
					if ( hrcvOrECLocation != 1 && hrcvOrECLocation != 2 ) {
						return "Molson Lounge or Escape";
					} else if ( hrcvOrECLocation == 1 ) {
						return "Molson Lounge";
					} else {
						return "Escape";
					}
				
				
				//case GCCPropertyShortCode.ECV:
				//    return "View Royal Patio";

				case GCCPropertyShortCode.ECV:
					return "Platinum Room";



				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
					return "The Well Public House";
				case GCCPropertyShortCode.CDC:
					return "Prospects Lounge";
				//case GCCPropertyShortCode.CNSH:
				//    return "Harbourfront Lounge";

				case GCCPropertyShortCode.CNSH:
					return "3Sixty";



				case GCCPropertyShortCode.SCTI:
					return "Windward Restaurant & Lounge";
				case GCCPropertyShortCode.CNB:
					return "Hub City Pub";
				case GCCPropertyShortCode.SCBE:
					return "Windward Restaurant & Lounge";

				case GCCPropertyShortCode.WDB:
					return "Windward Restaurant & Lounge";


				case GCCPropertyShortCode.GBH:
					return "Windward Restaurant & Lounge";



				case GCCPropertyShortCode.ECB:
					return "Brantford Live Entertainment";


				case GCCPropertyShortCode.ECF:
					return "Flamboro Live Entertainment";


			  
				case GCCPropertyShortCode.ECM:
					return "Mohawk Live Entertainment";



				default:
					return String.Empty;
			}
		}

		//20161214 French ver for CNB / SCTI

		public static string GetShowLoungeName_French(GCCPropertyShortCode shortCode, int hrcvOrECLocation = 0)
		{
			switch (shortCode)
			{
				case GCCPropertyShortCode.RR:
					//return "Lulu's Lounge";
					return "Curve Lounge";
				case GCCPropertyShortCode.HRCV:
					if (hrcvOrECLocation != 1 && hrcvOrECLocation != 2)
					{
						return "Asylum Gastro-Pub and Live Sound Stage or UnListed Buffet and Lounge";
					}
					else if (hrcvOrECLocation == 1)
					{
						return "Asylum Gastro-Pub and Live Sound Stage";
					}
					else
					{
						return "UnListed Buffet and Lounge";
					}
				case GCCPropertyShortCode.ECS:
					if (hrcvOrECLocation != 1 && hrcvOrECLocation != 2)
					{
						return "Molson Lounge or Escape";
					}
					else if (hrcvOrECLocation == 1)
					{
						return "Molson Lounge";
					}
					else
					{
						return "Escape";
					}
				//case GCCPropertyShortCode.ECV:
				//    return "View Royal Patio";

				case GCCPropertyShortCode.ECV:
					return "Platinum Room";

				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
					return "The Well Public House";
				case GCCPropertyShortCode.CDC:
					return "Prospects Lounge";
				//case GCCPropertyShortCode.CNSH:
				//    return "Harbourfront Lounge";

				case GCCPropertyShortCode.CNSH:
					return "3Sixty";
				case GCCPropertyShortCode.SCTI:
					return "Restaurant et Lounge Windward";
				case GCCPropertyShortCode.CNB:
					return "Pub Hub City";
				case GCCPropertyShortCode.SCBE:
					return "Restaurant et Lounge Windward";


				case GCCPropertyShortCode.WDB:
					return "Restaurant et Lounge Windward";


				case GCCPropertyShortCode.GBH:
					return "Restaurant et Lounge Windward";


				case GCCPropertyShortCode.ECB:
					return "Brantford Live Entertainment";


				case GCCPropertyShortCode.ECF:
					return "Flamboro Live Entertainment";



				case GCCPropertyShortCode.ECM:
					return "Mohawk Live Entertainment";


				default:
					return String.Empty;
			}
		}
		/// <summary>
		/// Returns true if the property has a food and beverage location for this mention. If true, the name of the location will be populated in the <paramref name="name"/> parameter.
		/// </summary>
		/// <param name="shortCode">The short code for the property.</param>
		/// <param name="mentionNumber">The mention number to check (1-13).</param>
		/// <param name="name">Returns the name of the location or an empty string if there isn't one.</param>
		public static bool HasFoodAndBev( GCCPropertyShortCode shortCode, int mentionNumber, out string name ) {
			name = GetFoodAndBevName( shortCode, mentionNumber );
			return !String.IsNullOrEmpty( name );
		}


		//20161214 French ver for CNB/SCTI
		public static bool HasFoodAndBev_French(GCCPropertyShortCode shortCode, int mentionNumber, out string name)
		{
			name = GetFoodAndBevName_French(shortCode, mentionNumber);
			return !String.IsNullOrEmpty(name);
		}


		/// <summary>
		/// Gets the region for a particular Casino.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		public static string GetCasinoRegion( GCCPropertyShortCode shortCode ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.CNSH:
				case GCCPropertyShortCode.CNSS:
					return "NS";
				case GCCPropertyShortCode.FL:
				case GCCPropertyShortCode.GD:
				case GCCPropertyShortCode.SSKD:
				case GCCPropertyShortCode.SCTI:
				case GCCPropertyShortCode.SCBE:
				case GCCPropertyShortCode.WDB:
				case GCCPropertyShortCode.AJA:
				case GCCPropertyShortCode.GBH:
				case GCCPropertyShortCode.ECB:
				case GCCPropertyShortCode.ECF:
				case GCCPropertyShortCode.ECGR:
				case GCCPropertyShortCode.ECM:

					return "ON";
				case GCCPropertyShortCode.GAG:
					return "WA";
				case GCCPropertyShortCode.CNB:
					return "NB";
				default:
					//Default region is BC
					return "BC";
			}
		}

		/// <summary>
		/// Gets the timezone for a particular Casino.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		public static string GetCasinoTimezone( GCCPropertyShortCode shortCode ) {
			switch ( shortCode ) {
				//case GCCPropertyShortCode.GCC:
				//case GCCPropertyShortCode.RR:
				//case GCCPropertyShortCode.HRCV:
				//case GCCPropertyShortCode.FD:
				//case GCCPropertyShortCode.HA:
				//case GCCPropertyShortCode.ECV:
				//case GCCPropertyShortCode.NAN:
				//case GCCPropertyShortCode.CCH:
				//case GCCPropertyShortCode.CMR:
				//case GCCPropertyShortCode.CDC:
				//case GCCPropertyShortCode.GAG:
				//    return "Pacific Standard Time";
				case GCCPropertyShortCode.CNSH:
				case GCCPropertyShortCode.CNSS:
				case GCCPropertyShortCode.CNB:
					return "Atlantic Standard Time";
				case GCCPropertyShortCode.FL:
				case GCCPropertyShortCode.GD:
				case GCCPropertyShortCode.SSKD:
				case GCCPropertyShortCode.SCTI:
				case GCCPropertyShortCode.SCBE:
				case GCCPropertyShortCode.WDB:
				case GCCPropertyShortCode.AJA:
				case GCCPropertyShortCode.GBH:
				case GCCPropertyShortCode.ECB:
				case GCCPropertyShortCode.ECF:
				case GCCPropertyShortCode.ECGR:
				case GCCPropertyShortCode.ECM:

					return "Eastern Standard Time";
				default:
					//Default timezone is pacific
					return "Pacific Standard Time";
			}
		}

		/// <summary>
		/// Gets the name of the food and beverage location for this casino, for a specific mention number (1-13). Returns blank for any unmatched values.
		/// </summary>
		/// <param name="shortCode">The short code to look up.</param>
		/// <param name="mentionNumber">The mention number.</param>
		public static string GetFoodAndBevName( GCCPropertyShortCode shortCode, int mentionNumber ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.RR:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Curve";
						case 3:
							return "Tramonto Restaurant";
						case 4:
							//return "Lulu's Lounge";
							return "Curve Lounge";
						case 5:
							return "The Buffet";
						case 6:
							return "Sea Harbour Seafood Restaurant";
						//case 7:
						//    return "Java Jacks Café";
							//20180205 replacing 
						case 7:
							return "Starbucks";

						case 8:
							return "International Food Court";
						case 9:
							return "Poker Room";
						case 10:
							return "Salon Privé";
						case 11:
							return "Dogwood Room";
						case 12:
							return "Jade Room";
						case 13:
							return "Phoenix Room";
					}
					break;
				case GCCPropertyShortCode.HRCV:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Asylum Gastro-Pub and Live Sound Stage";
						case 3:
							return "UnListed Buffet and Lounge";
						case 4:
							return "Neptune Noodle House";
						case 5:
							return "Fu Express Authentic Asian Cuisine";
						case 6:
							return "Roadies Burger Bar";
						case 7:
							return "Chip's Sandwich Shop";
						case 8:
							return "Fuel Café";
						case 9:
							return "Poker Room";
						case 10:
							return "Salon Privé";
					}
					break;
				case GCCPropertyShortCode.FD:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Homestretch Buffet";
						case 3:
							return "The Bridge";
						case 4:
							return "The Clubhouse Buffet";
						case 5:
							return "Casino Bar";
						case 6:
							return "Racebook";
					}
					break;
				case GCCPropertyShortCode.HA:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Eclipse Lounge";
						case 3:
							return "Silks Restaurant";
						case 4:
							return "Furlongs Eatery";
						case 5:
							return "Jeromes";
						case 6:
							return "George Royal Room";
						case 7:
							return "Concessions";
					}
					break;
				//case GCCPropertyShortCode.ECV:
				//    switch (mentionNumber)
				//    {
				//        case 1:
				//            return "Coffee Station / Gaming Floor";
				//        case 2:
				//            return "View Royal Restaurant";
				//        case 3:
				//            return "View Royal Patio";
				//    }
				//    break;



				case GCCPropertyShortCode.ECV:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Well Public House";
						case 3:
							return "Chi Express";
						case 4:
							return "The Diamond Buffet";
						case 5:
							return "1708 Quick Bites";
					}
					break;
				case GCCPropertyShortCode.NAN:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Black Diamond Bar & Grille";
					}
					break;
				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Well";
					}
					break;
				case GCCPropertyShortCode.CDC:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Prospects Lounge";
					}
					break;
				case GCCPropertyShortCode.CNSH:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "3Sixty Buffet";
						case 3:
							return "3Sixty Restaurant";
						case 4:
							return "Harbourfront Lounge";
						case 5:
							return "High Limit Gaming Area";
						case 6:
							return "Poker & Ponies Room";
					}
					break;
				case GCCPropertyShortCode.CNSS:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Celtic Junction Bar & Grille";
					}
					break;
				case GCCPropertyShortCode.GAG:
					switch ( mentionNumber ) {
						case 1:
							return "Bar / Restaurant at Great American Casino";
					}
					break;
				case GCCPropertyShortCode.ECS:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Homestretch Buffet";
						case 3:
							return "Racebook";
						case 4:
							return "Diamond Buffet";
						case 5:
							return "Foodies";
						case 6:
							return "Molson Lounge";
						case 7:
							return "Escape";
					}
					break;
				case GCCPropertyShortCode.SSKD:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Player's Lounge and Café";
					}
					break;
				case GCCPropertyShortCode.SCTI:
					switch ( mentionNumber ) {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Windward Restaurant & Lounge";
					}
					break;
				case GCCPropertyShortCode.CNB:
					switch (mentionNumber)   {
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Rue 333";
						case 3:
							return "Hub City Pub";
						case 4:
							return "Buffet";
						case 5:
							return "Room Service";
					}
					break;

				case GCCPropertyShortCode.SCBE:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Cart / Gaming Floor";
						case 2:
							return "Windward Restaurant & Lounge";
						case 3:
							return "The Buffet";
					}
					break;




				case GCCPropertyShortCode.AJA:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;
				case GCCPropertyShortCode.WDB:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Willows Dining Room";
						case 3:
							return "Willows Noodle Bar";
						case 4:
							return "Hoofbeats Lounge";
					}
					break;

				case GCCPropertyShortCode.GBH:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Waters Edge Buffet";
						case 3:
							return "Lucky Stone Bar & Grill";
						case 4:
							return "Heron Bar";
						case 5:
							return "Game Side Dining";
					}
					break;





				case GCCPropertyShortCode.ECB:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;



				case GCCPropertyShortCode.ECF:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Pavillion Restaurant";
						case 3:
							return "Moon Bar";
						case 4:
							return "Sunset Bar";
					}
					break;




				case GCCPropertyShortCode.ECGR:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;



				case GCCPropertyShortCode.ECM:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Marketplace";
						case 3:
							return "The Terrace Dining Room";
						case 4:
							return "Oscars Sports";
					   
					}
					break;


			}
			return String.Empty;
		}
		
		
	  


		//20161214 French ver for CNB / SCTI Foodandbevname
		public static string GetFoodAndBevName_French(GCCPropertyShortCode shortCode, int mentionNumber)
		{
			switch (shortCode)
			{
				case GCCPropertyShortCode.RR:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Curve";
						case 3:
							return "Tramonto Restaurant";
						case 4:
							//return "Lulu's Lounge";
							return "Curve Lounge";
						case 5:
							return "The Buffet";
						case 6:
							return "Sea Harbour Seafood Restaurant";
						//case 7:
						//    return "Java Jacks Café";
						case 7:
							return "Starbucks";
						case 8:
							return "International Food Court";
						case 9:
							return "Poker Room";
						case 10:
							return "Salon Privé";
						case 11:
							return "Dogwood Room";
						case 12:
							return "Jade Room";
						case 13:
							return "Phoenix Room";
					}
					break;
				case GCCPropertyShortCode.HRCV:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Asylum Gastro-Pub and Live Sound Stage";
						case 3:
							return "UnListed Buffet and Lounge";
						case 4:
							return "Stake Restaurant";
						case 5:
							return "Fu Express Authentic Asian Cuisine";
						case 6:
							return "Roadies Burger Bar";
						case 7:
							return "Chip's Sandwich Shop";
						case 8:
							return "Fuel Café";
						case 9:
							return "Poker Room";
						case 10:
							return "Salon Privé";
					}
					break;
				case GCCPropertyShortCode.FD:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Homestretch Buffet";
						case 3:
							return "The Bridge";
						case 4:
							return "The Clubhouse Buffet";
						case 5:
							return "Casino Bar";
						case 6:
							return "Racebook";
					}
					break;
				case GCCPropertyShortCode.HA:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Eclipse Lounge";
						case 3:
							return "Silks Restaurant";
						case 4:
							return "Furlongs Eatery";
						case 5:
							return "Jeromes";
						case 6:
							return "George Royal Room";
						case 7:
							return "Concessions";
					}
					break;
				//case GCCPropertyShortCode.ECV:
				//    switch (mentionNumber)
				//    {
				//        case 1:
				//            return "Coffee Station / Gaming Floor";
				//        case 2:
				//            return "View Royal Restaurant";
				//        case 3:
				//            return "View Royal Patio";
				//    }
				//    break;



				case GCCPropertyShortCode.ECV:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Well Public House";
						case 3:
							return "Chi Express";
						case 4:
							return "The Diamond Buffet";
						case 5:
							return "1708 Quick Bites";
					}
					break;


				case GCCPropertyShortCode.NAN:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Black Diamond Bar & Grille";
					}
					break;
				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Well";
					}
					break;
				case GCCPropertyShortCode.CDC:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Prospects Lounge";
					}
					break;
				case GCCPropertyShortCode.CNSH:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "3Sixty Buffet";
						case 3:
							return "3Sixty Restaurant";
						case 4:
							return "Harbourfront Lounge";
						case 5:
							return "High Limit Gaming Area";
						case 6:
							return "Salle de Poker & Poneys";
					}
					break;
				case GCCPropertyShortCode.CNSS:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Celtic Junction Bar & Grille";
					}
					break;
				case GCCPropertyShortCode.GAG:
					switch (mentionNumber)
					{
						case 1:
							return "Bar / Restaurant at Great American Casino";
					}
					break;
				case GCCPropertyShortCode.ECS:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Homestretch Buffet";
						case 3:
							return "Racebook";
						case 4:
							return "Diamond Buffet";
						case 5:
							return "Foodies";
						case 6:
							return "Molson Lounge";
						case 7:
							return "Escape";
					}
					break;
				case GCCPropertyShortCode.SSKD:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Player's Lounge and Café";
					}
					break;
				case GCCPropertyShortCode.SCTI:
					switch (mentionNumber)
					{
						case 1:
							return "station de café/salle de jeu";
						case 2:
							return "Restaurant et Lounge Windward";
					}
					break;
				case GCCPropertyShortCode.CNB:
					switch (mentionNumber)
					{
						case 1:
							return "station de café/salle de jeu";
						case 2:
							return "Rue 333";
						case 3:
							return "Pub Hub City";
						case 4:
							return "Buffet";
						case 5:
							return "Service à la chambre";
					}
					break;



				case GCCPropertyShortCode.SCBE:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Cart / Gaming Floor";
						case 2:
							return "Windward Restaurant & Lounge";
						case 3:
							return "The Buffet";
					}
					break;


				case GCCPropertyShortCode.AJA:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;
				case GCCPropertyShortCode.WDB:
					switch (mentionNumber)
					{
						case 1:
							return "Station de café/salle de jeu";
						case 2:
							return "Salle à manger Willows";
						case 3:
							return "Bar à mouilles Willows";
						case 4:
							return "Lounge Hoofbeats";
					}
					break;

				case GCCPropertyShortCode.GBH:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Waters Edge Buffet";
						case 3:
							return "Lucky Stone Bar & Grill";
						case 4:
							return "Heron Bar";
						case 5:
							return "Game Side Dining";
					}
					break;


				case GCCPropertyShortCode.ECB:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;



				case GCCPropertyShortCode.ECF:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
						case 3:
							return "Moon Bar";
						case 4:
							return "Sunset Bar";
					}
					break;




				case GCCPropertyShortCode.ECGR:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "Getaway Bar & Grill";
					}
					break;



				case GCCPropertyShortCode.ECM:
					switch (mentionNumber)
					{
						case 1:
							return "Coffee Station / Gaming Floor";
						case 2:
							return "The Marketplace";
						case 3:
							return "The Terrace Dining Room";
						case 4:
							return "Oscars Sports";

					}
					break;


			}
			return String.Empty;
		}
		
		
		
		/// <summary>
		/// Gets the name of a casino given the property.
		/// </summary>
		/// <param name="property">The property to get the name for.</param>
		public static string GetCasinoName( GCCProperty property ) {
			return GetCasinoName( (int)property );
		}


		public static string GetCasinoName_French(GCCProperty property)
		{
			return GetCasinoName_French((int)property);
		}


		/// <summary>
		/// Gets the name of a casino given the property ID.
		/// </summary>
		/// <param name="propertyID">The property ID of the casino to get the name for.</param>
		public static string GetCasinoName( int propertyID ) {
			switch ( propertyID ) {
				case 1:
					return "Great Canadian Gaming Corporation";
				case 2:
					return "River Rock Casino Resort";
				case 3:
					return "Hard Rock Casino Vancouver";
				case 4:
					return "Fraser Downs Racetrack & Casino";
				case 5:
					return "Hastings Racetrack & Casino";
				case 6:
					return "Elements Casino Victoria";
				case 7:
					return "Casino Nanaimo";
				case 8:
					return "Chances Chilliwack";
				case 9:
					return "Chances Maple Ridge";
				case 10:
					return "Chances Dawson Creek";
				case 11:
					return "Casino Nova Scotia - Halifax";
				case 12:
					return "Casino Nova Scotia - Sydney";
				case 13:
					return "Great American Casino";
				case 14:
					return "Elements Casino Surrey";
				case 15:
					return "Flamboro Downs";
				case 16:
					return "Georgian Downs";
				case 17:
					return "Shorelines Slots at Kawartha Downs";
				case 18:
					return "Shorelines Casino Thousand Islands";
				case 19:
					return "Casino New Brunswick";
				case 20:
					return "Shorelines Casino Belleville";

				case 21:
					return "Bingo Esquimalt";
				case 22:
					return "Casino Woodbine";
				case 23:
					return "Casino Ajax";
				case 24:
					return "Great Blue Heron Casino";


				case 25:
					return "Elements Casino Brantford";


				case 26:
					return "Elements Casino Flamboro";


				case 27:
					return "Elements Casino Grand River";


				case 28:
					return "Elements Casino Mohawk";

				case 91:
						return "Ontario Operations";


									case 92:
						return "NovaScotia and NewBrunswuick Operations";


									case 93:
						return "British Columbia Operations";


									case 94:
						return "Washington State Operations";


									case 95:
						return "Corporate Office";



	






				default:
					return String.Empty;
			}
		}




		//20161214 French ver for CNB and SCTI

		public static string GetCasinoName_French(int propertyID)
		{
			switch (propertyID)
			{
				case 1:
					return "Great Canadian Gaming Corporation";
				case 2:
					return "River Rock Casino Resort";
				case 3:
					return "Hard Rock Casino Vancouver";
				case 4:
					return "Fraser Downs Racetrack & Casino";
				case 5:
					return "Hastings Racetrack & Casino";
				case 6:
					return "Elements Casino Victoria";
				case 7:
					return "Casino Nanaimo";
				case 8:
					return "Chances Chilliwack";
				case 9:
					return "Chances Maple Ridge";
				case 10:
					return "Chances Dawson Creek";
				case 11:
					return "Casino Nova Scotia - Halifax";
				case 12:
					return "Casino Nova Scotia - Sydney";
				case 13:
					return "Great American Casino";
				case 14:
					return "Elements Casino Surrey";
				case 15:
					return "Flamboro Downs";
				case 16:
					return "Georgian Downs";
				case 17:
					return "Shorelines Slots at Kawartha Downs";
				case 18:
					return "Casino Shorelines à Thousand Islands ";
				case 19:
					return "Casino Nouveau-Brunswick";
				case 20:
					return "Shorelines Casino Belleville";

				case 21:
					return "Bingo Esquimalt";

				case 22:
					return "Casino Woodbine";
				case 23:
					return "Casino Ajax";
				case 24:
					return "Great Blue Heron Casino";


				case 25:
					return "Elements Casino Brantford";


				case 26:
					return "Elements Casino Flamboro";


				case 27:
					return "Elements Casino Grand River";


				case 28:
					return "Elements Casino Mohawk";


				case 91:
					return "Ontario Operations";


				case 92:
					return "NovaScotia and NewBrunswuick Operations";


				case 93:
					return "British Columbia Operations";


				case 94:
					return "Washington State Operations";


				case 95:
					return "Corporate Office";




				default:
					return String.Empty;
			}
		}



		/// <summary>
		/// Gets the name of the Players Club Name given the property ID.
		/// </summary>
		/// <param name="propertyID">The property ID of the casino to get the name for.</param>
		public static string GetPlayersClubName( int propertyID ) {
			switch ( propertyID ) {
				case 1:
				case 11:
					return "Players Club";
				case 12:
					return "Players Club";
				case 2:
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 14:
					return "Encore Rewards";
				case 4:
				case 5:
					return "Encore Rewards or HorsePlayer Interactive";
				case 13:
					return "Great Rewards Club";
				case 17:
					return "Winner's Circle";
				case 18:
					return "Axis Rewards";
				case 19:
					return "Rewards Club";

				case 20:
					return "AXIS Rewards";

				case 22:
					return "Winner's Circle";

				case 23:
					return "Winner's Circle";

				case 24:
					return "Rapid Rewards";


				case 25:
					return "Winner's Circle";


				case 26:
					return "Winner's Circle";

				case 27:
					return "Winner's Circle";

				case 28:
					return "Winner's Circle";
				
				default:
					return String.Empty;
			}
		}

		//20161214 French Ver for CNB ans scti
		public static string GetPlayersClubName_French(int propertyID)
		{
			switch (propertyID)
			{
				case 1:
				case 11:
					return "Players Club";
				case 12:
					return "Players Club";
				case 2:
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 14:
					return "Encore Rewards";
				case 4:
				case 5:
					return "Encore Rewards or HorsePlayer Interactive";
				case 13:
					return "Great Rewards Club";
				case 17:
					return "cercle des gagnants";
				case 18:
					return "Axis Rewards";
				case 19:
					return "Club de récompenses";
				case 20:
					return "AXIS Rewards";

				case 22:
					return "cercle des gagnants";

				case 23:
					return "cercle des gagnants";

				case 24:
					return "Récompenses rapides";



				case 25:
					return "cercle des gagnants";


				case 26:
					return "cercle des gagnants";

				case 27:
					return "cercle des gagnants";

				case 28:
					return "cercle des gagnants";
				default:
					return String.Empty;
			}
		}

		
		
		/// <summary>
		/// Returns the text description of the survey prize.
		/// </summary>
		/// <param name="property"></param>
		public static string GetSurveyPrize( GCCPropertyShortCode property ) {
			switch ( property ) {
				case GCCPropertyShortCode.RR:
				case GCCPropertyShortCode.HRCV:
				case GCCPropertyShortCode.FD:
				case GCCPropertyShortCode.HA:
				case GCCPropertyShortCode.ECV:
				case GCCPropertyShortCode.NAN:
				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
				case GCCPropertyShortCode.CDC:
				case GCCPropertyShortCode.ECS:
					return "ONE OF TEN $100 GIFT CARDS, or the GRAND PRIZE of $500!";
				case GCCPropertyShortCode.CNSH:
					return "Dinner For Four at 3Sixty Buffet";
				case GCCPropertyShortCode.CNSS:
					return "Dinner For Four at Celtic Junction Bar & Grille";
				case GCCPropertyShortCode.GAG:
				case GCCPropertyShortCode.SSKD:
				case GCCPropertyShortCode.SCTI:
				case GCCPropertyShortCode.SCBE:
				case GCCPropertyShortCode.WDB:
				case GCCPropertyShortCode.AJA:
				case GCCPropertyShortCode.GBH:
				case GCCPropertyShortCode.ECB:
				case GCCPropertyShortCode.ECF:
				case GCCPropertyShortCode.ECGR:
				case GCCPropertyShortCode.ECM:
					return "$100";
				case GCCPropertyShortCode.CNB:
					return "A Dinner Buffet for 4";
				default:
					return String.Empty;
			}
		}





		//20161214 - French ver for CNB and SCTI


		public static string GetSurveyPrize_French(GCCPropertyShortCode property)
		{
			switch (property)
			{
				case GCCPropertyShortCode.RR:
				case GCCPropertyShortCode.HRCV:
				case GCCPropertyShortCode.FD:
				case GCCPropertyShortCode.HA:
				case GCCPropertyShortCode.ECV:
				case GCCPropertyShortCode.NAN:
				case GCCPropertyShortCode.CCH:
				case GCCPropertyShortCode.CMR:
				case GCCPropertyShortCode.CDC:
				case GCCPropertyShortCode.ECS:
					return "ONE OF TEN $100 GIFT CARDS, or the GRAND PRIZE of $500!";
				case GCCPropertyShortCode.CNSH:
					return "Dinner For Four at 3Sixty Buffet";
				case GCCPropertyShortCode.CNSS:
					return "Dinner For Four at Celtic Junction Bar & Grille";
				case GCCPropertyShortCode.GAG:
				case GCCPropertyShortCode.SSKD:
				case GCCPropertyShortCode.SCTI:
				case GCCPropertyShortCode.SCBE:
				case GCCPropertyShortCode.WDB:
				case GCCPropertyShortCode.AJA:
				case GCCPropertyShortCode.GBH:
				case GCCPropertyShortCode.ECB:
				case GCCPropertyShortCode.ECF:
				case GCCPropertyShortCode.ECGR:
				case GCCPropertyShortCode.ECM:
					return "100 $";
				case GCCPropertyShortCode.CNB:
					return "Vous pourriez gagner un souper-buffet pour quatre personnes!";
				default:
					return String.Empty;
			}
		}



		/// <summary>
		/// Gets the casino header image.
		/// </summary>
		/// <param name="shortCode">The short code of the property.</param>
		public static string GetCasinoHeaderImage( GCCPropertyShortCode shortCode ) {
			switch ( shortCode ) {
				case GCCPropertyShortCode.GCC:
					return "CorporateHeader.jpg";
			   
				case GCCPropertyShortCode.RR:
					return "CorporateHeader.jpg";
				  //  return "RiverRockHeader.jpg";
				case GCCPropertyShortCode.HRCV:
					return "HardRockHeader.jpg";
				case GCCPropertyShortCode.FD:
					return "FraserDownsHeader.jpg";
				case GCCPropertyShortCode.HA:
					return "HastingsRaceCourseHeader.jpg";
				case GCCPropertyShortCode.ECV:
					return "ElementsCasinoVictoriaHeader.jpg";
				case GCCPropertyShortCode.NAN:
					return "CasinoNanaimoHeader.jpg";
				case GCCPropertyShortCode.CCH:
					return "ChancesChilliwackHeader.jpg";
				case GCCPropertyShortCode.CMR:
					return "ChancesMapleRidgeHeader.jpg";
				case GCCPropertyShortCode.CDC:
					return "ChancesDawsonCreekHeader.jpg";
				case GCCPropertyShortCode.CNSH:
					return "CasinoNovaScotiaHalifaxHeader.jpg";
				case GCCPropertyShortCode.CNSS:
					return "CasinoNovaScotiaSydneyHeader.jpg";
				case GCCPropertyShortCode.GAG:
					return "GreatAmericanGamingHeader.gif";
				case GCCPropertyShortCode.ECS:
					return "ElementsCasinoHeader.jpg";
				//TODO: Get casino headers for FL and GD
				case GCCPropertyShortCode.SSKD:
					return "KawarthaDownsHeader.jpg";
				case GCCPropertyShortCode.SCTI:
					return "ThousandIslandsHeader.jpg";
				case GCCPropertyShortCode.CNB:
					return "CasinoNewBrunswickHeader.jpg";
				case GCCPropertyShortCode.SCBE:
					return "Belleville.jpg";
				case GCCPropertyShortCode.BSQ:
					return "CorporateHeader.jpg";
				case GCCPropertyShortCode.WDB:
					return "WDBHeader.jpg";

				case GCCPropertyShortCode.AJA:
					return "AJAHeader.jpg";

				case GCCPropertyShortCode.GBH:
					return "GBHHeader.jpg";
				case GCCPropertyShortCode.ECB:
					return "ECBHeader.jpg";
				case GCCPropertyShortCode.ECF:
					return "ECFHeader.jpg";
				case GCCPropertyShortCode.ECGR:
					return "ECGRHeader.jpg";
				case GCCPropertyShortCode.ECM:
					return "ECMHeader.jpg";
				default:
					return String.Empty;
			}
		}

		/// <summary>
		/// Returns the associated property enum value from the property short code enum value.
		/// </summary>
		/// <param name="code">The short code value.</param>
		public static GCCProperty GetPropertyFromShortCode( GCCPropertyShortCode code ) {
			return (GCCProperty)( (int)code );
		}
		/// <summary>
		/// Returns the associated property short code enum value from the property enum value.
		/// </summary>
		/// <param name="code">The short code value.</param>
		public static GCCPropertyShortCode GetPropertyShortCodeFromProperty( GCCProperty property ) {
			return (GCCPropertyShortCode)( (int)property );
		}
	}
}
