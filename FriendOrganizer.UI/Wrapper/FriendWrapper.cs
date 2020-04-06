using FriendOrganizer.Model;
using System;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Wrapper
{
    /// <summary>
    /// We use this class in FriendDetailViewModel
    /// </summary>
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                //ValidateProperty(nameof(FirstName));
            }
        }        
        //private void ValidateProperty(string propertyName)
        //{
        //    ClearErrors(propertyName);
        //    switch (propertyName)
        //    {
        //        case nameof(FirstName):
        //            if(string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
        //            {
        //                AddError(propertyName, "Robots are not valid friends");
        //            }
        //            break;
        //    }
        //}

        public string LastName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                /*Model.LastName = value;
                OnPropertyChanged();*/
            }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid friends";
                    }
                    break;
            }
        }
        }
}
