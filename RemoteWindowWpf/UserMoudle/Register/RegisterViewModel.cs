﻿
using Controls.Dialogs;
using Dispath;
using Dispath.MoudleInterface;
using LYL.Logic.Machine;
using MaterialDesignThemes.Wpf;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserMoudle.Register
{
    public class RegisterViewModel : ViewModelBase
    {
        public string userName { get; set; }
        public string pwd { get; set; }
        public string email { get; set; }

        public string checkId { get; set; }
        public string checkCode { get; set; }

        public bool isSendCheckRequest
        {
            get
            {
                if (string.IsNullOrEmpty(checkId))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Visibility requestVisibility
        {
            get
            {
                if (isSendCheckRequest == true)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public Visibility checkVisibility
        {
            get
            {
                if (isSendCheckRequest == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }


        private DelegateCommand _requestCommand => new DelegateCommand(this.requestMethod);
        public DelegateCommand requestCommand { get { return this._requestCommand; } }
        public async void requestMethod(object param)
        {
            var loadinghandle = this.ShowLoadingWindow();
            try
            {
                this.checkId = await Task.Run(() => { return UserLogic.requestRegisterUser(userName, pwd, email); });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                loadinghandle.Close();
            }

            this.OnPropertyChanged(nameof(this.checkId));
            this.OnPropertyChanged(nameof(this.isSendCheckRequest));
            this.OnPropertyChanged(nameof(this.requestVisibility));
            this.OnPropertyChanged(nameof(this.checkVisibility));
        }

        private DelegateCommand _checkCommand => new DelegateCommand(this.checkMethod);
        public DelegateCommand checkCommand { get { return this._checkCommand; } }
        public void checkMethod(object param)
        {
            if (UserLogic.checkRegisterUser(checkId, checkCode) == true)
            {
                ErrorDialogCon con = new ErrorDialogCon();
                con.ErrorMsg = "注册成功，请登录";
                //show the dialog
                con.PopupDialog();
                NavigationHelper.NavigatedToView("用户登录");
            }
        }

       
    }
}
