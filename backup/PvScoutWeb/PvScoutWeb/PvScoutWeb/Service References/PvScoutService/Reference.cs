﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 3.0.40818.0
// 
namespace PvScoutWeb.PvScoutService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PvModuleActual", Namespace="http://schemas.datacontract.org/2004/07/Solr.Pvscout.Business.Entity")]
    public partial class PvModuleActual : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int HeightField;
        
        private int WidthField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Height {
            get {
                return this.HeightField;
            }
            set {
                if ((this.HeightField.Equals(value) != true)) {
                    this.HeightField = value;
                    this.RaisePropertyChanged("Height");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Width {
            get {
                return this.WidthField;
            }
            set {
                if ((this.WidthField.Equals(value) != true)) {
                    this.WidthField = value;
                    this.RaisePropertyChanged("Width");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PvModule", Namespace="http://schemas.datacontract.org/2004/07/Solr.Pvscout.Business.Entity")]
    public partial class PvModule : object, System.ComponentModel.INotifyPropertyChanged {
        
        private PvScoutWeb.PvScoutService.PvModuleActual ObjPvModuleActualField;
        
        private PvScoutWeb.PvScoutService.PvModuleVirtual ObjPvModuleVirtualField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PvScoutWeb.PvScoutService.PvModuleActual ObjPvModuleActual {
            get {
                return this.ObjPvModuleActualField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjPvModuleActualField, value) != true)) {
                    this.ObjPvModuleActualField = value;
                    this.RaisePropertyChanged("ObjPvModuleActual");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PvScoutWeb.PvScoutService.PvModuleVirtual ObjPvModuleVirtual {
            get {
                return this.ObjPvModuleVirtualField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjPvModuleVirtualField, value) != true)) {
                    this.ObjPvModuleVirtualField = value;
                    this.RaisePropertyChanged("ObjPvModuleVirtual");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PvModuleVirtual", Namespace="http://schemas.datacontract.org/2004/07/Solr.Pvscout.Business.Entity")]
    public partial class PvModuleVirtual : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int HeightField;
        
        private int WidthField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Height {
            get {
                return this.HeightField;
            }
            set {
                if ((this.HeightField.Equals(value) != true)) {
                    this.HeightField = value;
                    this.RaisePropertyChanged("Height");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Width {
            get {
                return this.WidthField;
            }
            set {
                if ((this.WidthField.Equals(value) != true)) {
                    this.WidthField = value;
                    this.RaisePropertyChanged("Width");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PvScoutService.IPvScoutService")]
    public interface IPvScoutService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPvScoutService/GetPvModulePosition", ReplyAction="http://tempuri.org/IPvScoutService/GetPvModulePositionResponse")]
        System.IAsyncResult BeginGetPvModulePosition(PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual, System.AsyncCallback callback, object asyncState);
        
        PvScoutWeb.PvScoutService.PvModule EndGetPvModulePosition(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPvScoutServiceChannel : PvScoutWeb.PvScoutService.IPvScoutService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetPvModulePositionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetPvModulePositionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public PvScoutWeb.PvScoutService.PvModule Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((PvScoutWeb.PvScoutService.PvModule)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PvScoutServiceClient : System.ServiceModel.ClientBase<PvScoutWeb.PvScoutService.IPvScoutService>, PvScoutWeb.PvScoutService.IPvScoutService {
        
        private BeginOperationDelegate onBeginGetPvModulePositionDelegate;
        
        private EndOperationDelegate onEndGetPvModulePositionDelegate;
        
        private System.Threading.SendOrPostCallback onGetPvModulePositionCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public PvScoutServiceClient() {
        }
        
        public PvScoutServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PvScoutServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PvScoutServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PvScoutServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetPvModulePositionCompletedEventArgs> GetPvModulePositionCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PvScoutWeb.PvScoutService.IPvScoutService.BeginGetPvModulePosition(PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetPvModulePosition(objPvModuleActual, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PvScoutWeb.PvScoutService.PvModule PvScoutWeb.PvScoutService.IPvScoutService.EndGetPvModulePosition(System.IAsyncResult result) {
            return base.Channel.EndGetPvModulePosition(result);
        }
        
        private System.IAsyncResult OnBeginGetPvModulePosition(object[] inValues, System.AsyncCallback callback, object asyncState) {
            PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual = ((PvScoutWeb.PvScoutService.PvModuleActual)(inValues[0]));
            return ((PvScoutWeb.PvScoutService.IPvScoutService)(this)).BeginGetPvModulePosition(objPvModuleActual, callback, asyncState);
        }
        
        private object[] OnEndGetPvModulePosition(System.IAsyncResult result) {
            PvScoutWeb.PvScoutService.PvModule retVal = ((PvScoutWeb.PvScoutService.IPvScoutService)(this)).EndGetPvModulePosition(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetPvModulePositionCompleted(object state) {
            if ((this.GetPvModulePositionCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetPvModulePositionCompleted(this, new GetPvModulePositionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetPvModulePositionAsync(PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual) {
            this.GetPvModulePositionAsync(objPvModuleActual, null);
        }
        
        public void GetPvModulePositionAsync(PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual, object userState) {
            if ((this.onBeginGetPvModulePositionDelegate == null)) {
                this.onBeginGetPvModulePositionDelegate = new BeginOperationDelegate(this.OnBeginGetPvModulePosition);
            }
            if ((this.onEndGetPvModulePositionDelegate == null)) {
                this.onEndGetPvModulePositionDelegate = new EndOperationDelegate(this.OnEndGetPvModulePosition);
            }
            if ((this.onGetPvModulePositionCompletedDelegate == null)) {
                this.onGetPvModulePositionCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetPvModulePositionCompleted);
            }
            base.InvokeAsync(this.onBeginGetPvModulePositionDelegate, new object[] {
                        objPvModuleActual}, this.onEndGetPvModulePositionDelegate, this.onGetPvModulePositionCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override PvScoutWeb.PvScoutService.IPvScoutService CreateChannel() {
            return new PvScoutServiceClientChannel(this);
        }
        
        private class PvScoutServiceClientChannel : ChannelBase<PvScoutWeb.PvScoutService.IPvScoutService>, PvScoutWeb.PvScoutService.IPvScoutService {
            
            public PvScoutServiceClientChannel(System.ServiceModel.ClientBase<PvScoutWeb.PvScoutService.IPvScoutService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetPvModulePosition(PvScoutWeb.PvScoutService.PvModuleActual objPvModuleActual, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = objPvModuleActual;
                System.IAsyncResult _result = base.BeginInvoke("GetPvModulePosition", _args, callback, asyncState);
                return _result;
            }
            
            public PvScoutWeb.PvScoutService.PvModule EndGetPvModulePosition(System.IAsyncResult result) {
                object[] _args = new object[0];
                PvScoutWeb.PvScoutService.PvModule _result = ((PvScoutWeb.PvScoutService.PvModule)(base.EndInvoke("GetPvModulePosition", _args, result)));
                return _result;
            }
        }
    }
}