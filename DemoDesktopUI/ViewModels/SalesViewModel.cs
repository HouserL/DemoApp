using Caliburn.Micro;
using DemoDataManager.Library.Models;
using DemoDesktopUI.Library.API;
using DemoDesktopUI.Library.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        IConfigHelper _configHelper;
        
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _configHelper = configHelper;   
            _productEndpoint = productEndpoint;                 
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        public async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }
        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }
        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value; 
                NotifyOfPropertyChange(() =>Cart);
            }
        }


        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                // Replace with calculation
                return CalculateSubTotal().ToString("C");
            }
        }
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }
        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal highTaxRate = _configHelper.GetHighTaxRate()/100;
            decimal lowTaxRate = _configHelper.GetLowTaxRate()/100;
            foreach (var item in Cart)
            {
                if (item.Product.IsHighTaxable)
                {
                    taxAmount += (item.Product.RetailPrice * item.QuantityInCart * highTaxRate);
                }
                else if (item.Product.IsLowTaxable)
                {
                    taxAmount += (item.Product.RetailPrice * item.QuantityInCart * lowTaxRate);
                }
            }
            return taxAmount;
        }
        public string Total
        {
            get
            {
                return (CalculateSubTotal() + CalculateTax()).ToString("C");
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.ResetItem(Cart.IndexOf(existingItem));
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity,
                };
                Cart.Add(item);
            }
            
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);


        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // Make sure there is an item quantity

                return output;
            }
        }

        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // Make sure something is in cart

                return output;
            }
        }

        public void CheckOut()
        {

        }
    }
}