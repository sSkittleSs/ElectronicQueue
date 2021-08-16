using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Ticket : BaseVM
    {
        #region Fields
        private string dateTime;
        private int ticketID;
        private int doctorID;
        private int? userID;
        #endregion

        #region Properties
        public string DateTime
        {
            set => SetProperty(ref dateTime, value);
            get => dateTime;
        }

        public int TicketID
        {
            set => SetProperty(ref ticketID, value);
            get => ticketID;
        }

        public int DoctorID
        {
            set => SetProperty(ref doctorID, value);
            get => doctorID;
        }

        public int? UserID
        {
            set => SetProperty(ref userID, value);
            get => userID;
        }
        #endregion

        #region InnerEnumerations
        public enum TicketProperties { DateTime, DoctorID, UserID }
        #endregion

        #region Constructors
        public Ticket() { }

        public Ticket(string dateTime, int ticketID, int doctorID, int? userID)
        {
            DateTime = dateTime;
            TicketID = ticketID;
            DoctorID = doctorID;
            UserID = userID;
        }

        public Ticket(string dateTime, int doctorID, int? userID)
        {
            DateTime = dateTime;
            DoctorID = doctorID;
            UserID = userID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Date: {DateTime}\nDoctorID: {DoctorID}\nUserID: {UserID}\nTicketID: {TicketID}";

        public static string GetPropertyInStringByTicketID(int ticketID, TicketProperties property)
        {
            switch (property)
            {
                case TicketProperties.DateTime:
                    return MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.TicketID == ticketID)?.DateTime;
                case TicketProperties.DoctorID:
                    return MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.TicketID == ticketID)?.DoctorID.ToString();
                case TicketProperties.UserID:
                    return MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.TicketID == ticketID)?.UserID.ToString();
                default:
                    return "NONE";
            }
        }
        #endregion
    }
}
