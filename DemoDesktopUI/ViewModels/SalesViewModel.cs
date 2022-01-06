using Caliburn.Micro;
using DemoDesktopUI.Library.API;
using DemoDesktopUI.Library.Helpers;
using DemoDesktopUI.Library.Models;
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
        ISaleEndPoint _saleEndPoint;
        
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint)
        {
            _saleEndPoint = saleEndPoint;
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

        private CartItemModel _selectedCartItem;

        public CartItemModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set 
            { 
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
            decimal highTaxAmount = 0;
            decimal lowTaxAmount = 0;
            decimal highTaxRate = _configHelper.GetHighTaxRate()/100;
            decimal lowTaxRate = _configHelper.GetLowTaxRate()/100;

            highTaxAmount = Cart
                .Where(x => x.Product.IsHighTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * highTaxRate);

            lowTaxAmount = Cart    
                .Where(x => x.Product.IsLowTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * lowTaxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsHighTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * highTaxRate);
            //    }
            //    else if (item.Product.IsLowTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * lowTaxRate);
            //    }
            //}

            return (highTaxAmount + lowTaxAmount);
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
            NotifyOfPropertyChange(() => CanCheckOut);

        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // Make sure there is an item quantity
                if (SelectedCartItem != null)
                {
                    output = true;
                }
                return output;
            }
        }

        public void RemoveFromCart()
        {
            Cart.Remove(SelectedCartItem);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // Make sure something is in cart
                if (Cart.Count > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantitiy = item.QuantityInCart
                });
            }

            await _saleEndPoint.PostSale(sale);
        }
    }
}