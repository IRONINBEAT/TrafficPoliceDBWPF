using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Inspector : ViewModelBase
    {
        private int _id;
        private string _surname;
        private string _name;
        private string _patronymic;
        private int _postId;
        private string _postTitle;

        [DisplayName("ID")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [DisplayName("Фамилия")]
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        [DisplayName("Имя")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [DisplayName("Отчество")]
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public int PostId
        {
            get => _postId;
            set
            {
                _postId = value;
                OnPropertyChanged(nameof(PostId));
            }
        }

        [DisplayName("Должность")]
        public string PostTitle
        {
            get => _postTitle;
            set
            {
                _postTitle = value;
                OnPropertyChanged(nameof(PostTitle));
            }
        }
    }
}
