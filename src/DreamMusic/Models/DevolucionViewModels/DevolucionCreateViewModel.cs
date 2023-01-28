using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamMusic.Models.DevolucionViewModels
{
    public class DevolucionCreateViewModel : IValidatableObject
    {
        public virtual string Name
        {
            get;
            set;
        }

        [Display(Name = "Apellido 1")]
        public virtual string FirstSurname
        {
            get;
            set;
        }

        [Display(Name = "Apellido 2")]
        public virtual string SecondSurname
        {
            get;
            set;
        }

        public string CustomerId
        {
            get;
            set;
        }

        public double TotalPrice
        {
            get;
            set;
        }

        public DateTime DevolucionDate
        {
            get;
            set;
        }

        public virtual IList<DevolucionItemViewModel> DevolucionItems
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, añada su direccion de envio")]

        public String DeliveryAddress
        {
            get;
            set;
        }

        [Display(Name = "Metodo de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, seleccione tu método de pago para devolver")]
        public String PaymentMethod
        {
            get;
            set;
        }

        public DevolucionCreateViewModel()
        {
            DevolucionItems = new List<DevolucionItemViewModel>();
        }


        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public string Prefix { get; set; }


        [StringLength(7, MinimumLength = 6)]

        public string Phone { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "You have to introduce 16 numbers")]
        [Display(Name = "Credit Card")]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? ExpirationDate { get; set; }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is DevolucionCreateViewModel model)
                result = Name == model.Name &&
                  FirstSurname == model.FirstSurname &&
                  SecondSurname == model.SecondSurname &&
                  CustomerId == model.CustomerId &&
                  TotalPrice == model.TotalPrice &&
                  DevolucionDate == model.DevolucionDate &&
                  DeliveryAddress == model.DeliveryAddress &&
                  PaymentMethod == model.PaymentMethod &&
                 Email == model.Email &&
                Prefix == model.Prefix &&
                Phone == model.Phone &&
                CreditCardNumber == model.CreditCardNumber &&
                CCV == model.CCV &&
                ExpirationDate == model.ExpirationDate;
            else
                return false;
            for (int i = 0; i < this.DevolucionItems.Count; i++)
                result = result && (this.DevolucionItems[i].Equals(model.DevolucionItems[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentMethod == "CreditCard")
            {
                if (CreditCardNumber == null)
                    yield return new ValidationResult("Por favor, rellena tu número de tarjeta de crédito para tu reembolso con tarjeta de crédito",
                        new[] { nameof(CreditCardNumber) });
                if (CCV == null)
                    yield return new ValidationResult("Por favor, rellena tu CCV para tu reembolso con tarjeta de crédito",
                        new[] { nameof(CCV) });
                if (ExpirationDate == null)
                    yield return new ValidationResult("Por favor, rellena la Fecha De expiración para tu reembolso con tarjeta de crédito",
                        new[] { nameof(ExpirationDate) });
            }
            else
            {
                if (Email == null)
                    yield return new ValidationResult("Por favor, rellena con tu Email para tu reembolso con PayPal",
                        new[] { nameof(Email) });
                if (Prefix == null)
                    yield return new ValidationResult("Por favor, rellena con tu prefijo para tu reembolso con PayPal",
                        new[] { nameof(Prefix) });
                if (Phone== null)
                    yield return new ValidationResult("Por favor, rellena con tu teléfono para tu reembolso con PayPal",
                        new[] { nameof(Phone) });
            }

            //it is checked whether quantity is higher than 0 for at least one movie
            if (DevolucionItems.Sum(pi => pi.Quantity) <= 0)
                yield return new ValidationResult("Por favor, selecciona una cantidad mayor que 0 para al menos un disco",
                     new[] { nameof(DevolucionItems) });
        }

    }



    public class DevolucionItemViewModel
    {
        public virtual int DiscoID
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Primer nombre no puede ser mayor que 50 caracteres.")]
        public virtual String Title
        {
            get;
            set;
        }

        [Display(Name = "Precio de devolucion")]
        public virtual int PriceDeDevolucion
        {
            get;
            set;
        }

        public virtual String Genre
        {
            get;
            set;
        }

        [Required]
        public virtual int Quantity
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            DevolucionItemViewModel purchaseItem = obj as DevolucionItemViewModel;
            bool result = false;
            if ((DiscoID == purchaseItem.DiscoID)
                && (this.PriceDeDevolucion == purchaseItem.PriceDeDevolucion)
                    && (this.Quantity == purchaseItem.Quantity)
                    && (this.Title == purchaseItem.Title))
                result = true;
            return result;
        }


    }



}

