namespace WebHost.DomainModels
{
    using System;
    using System.Runtime.Serialization;

    public abstract class DateSerizalizer
    {
        private DateTime _startDate;
        private DateTime _endDate;

        [IgnoreDataMember]
        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        [DataMember]
        public string StartDateString
        {
            get
            {
                return this._startDate.ToString();
            }
            set
            {
                this._startDate = DateTime.ParseExact(value, "dd/MM/yyyy", null);
                //DateTime date = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
            }
        }

        [IgnoreDataMember]
        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        [DataMember]
        public string EndDateString
        {
            get
            {
                return this._endDate.ToString();
            }
            set
            {
                this._endDate = DateTime.ParseExact(value, "dd/MM/yyyy", null); ;
                //DateTime date = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
            }
        }
    }
}