﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace covertFiles {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("covertFiles.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&apos;lime&apos;&gt;&lt;/body&gt;
        ///&lt;SCRIPT LANGUAGE=&quot;JavaScript&quot;&gt;
        ///&lt;!-- 
        ///function jump()
        ///{
        ///	window.location.href = &quot;res://close&quot;;
        ///}
        ///// --&gt;
        ///&lt;/SCRIPT&gt;
        ///&lt;table border=&apos;2&apos; BORDERCOLOR=&apos;FF0000&apos; onclick=&apos;jump();&apos;&gt;
        ///&lt;tr bgcolor=&apos;FF0000&apos;&gt;
        ///	&lt;td&gt;
        ///		&lt;table border=&apos;0&apos; frame=&apos;border&apos; color=&apos;FF0000&apos; bgcolor=&apos;FFA000&apos;&gt;
        ///		&lt;tr&gt;
        ///			&lt;td colspan=&apos;2&apos; align=&apos;center&apos; bgcolor=&apos;FF0000&apos;&gt;
        ///				About CovertFile
        ///			&lt;/td&gt;
        ///		&lt;/tr&gt;
        ///		&lt;tr&gt;
        ///			&lt;td&gt;Application :&lt;/td&gt; &lt;td&gt;CovertFile&lt;/td&gt;
        ///		&lt;/tr&gt;
        ///		&lt;tr&gt;
        ///			&lt;td&gt;Version :&lt;/td&gt; &lt;td&gt;$ve [rest of string was truncated]&quot;;.
        /// </summary>
        public static string About {
            get {
                return ResourceManager.GetString("About", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;File Lock Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;
        ///.
        /// </summary>
        public static string FileLock {
            get {
                return ResourceManager.GetString("FileLock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Hamming Data Correction Code Help&lt;/h3&gt;&lt;br&gt;
        ///Hamming Data Correction Code is a linear error-correcting code. 
        ///It used as an error control for data transmition. 
        ///This method adds redundant data to messages. 
        ///This allows the receiver to detect and correct errors.
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://FileLock&quot; &gt;Lock File&lt;/a&gt;&lt;br&gt;
        ///&lt;a href = &quot;res://SharedCpuUsage&quot; &gt;Shared CPU Usage&lt;/a&gt;&lt;br&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet D [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HammingCorrection {
            get {
                return ResourceManager.GetString("HammingCorrection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Covert Channel Help&lt;/h3&gt;&lt;br&gt;
        ///There are two kind of Covert Channel  
        ///the Storage and the Timing. Here are implement 
        ///four Covert Channel two from each kind. Specific
        /// from Storage are implement &lt;a href = &quot;res://SharedFile&quot; &gt;Shared File&lt;/a&gt; and &lt;a href = &quot;res://FileLock&quot; &gt;Lock File&lt;/a&gt;
        /// and from Timing &lt;a href = &quot;res://SharedCpuUsage&quot; &gt;Shared CPU Usage&lt;/a&gt; and &lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.
        ///.
        /// </summary>
        public static string helpMain {
            get {
                return ResourceManager.GetString("helpMain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Interval Adjusting Help&lt;/h3&gt;&lt;br&gt;
        ///This is a method to solve Synchronization Problem.
        ///Rather than slow down the transmission by introducing
        ///silent periods in which no transfer occurs, the channel can
        ///adapt to the changes gradually as the network conditions
        ///change. In our interval adjusting scheme, the receiver closely
        ///monitors the time each packet arrives and compares it to the
        ///projected ideal case (the expected arrival time of the next
        ///packet) based on the current  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string IntervalAdjust {
            get {
                return ResourceManager.GetString("IntervalAdjust", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Message TimeOut Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://zerobitdelay&quot; &gt;Zero Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://onebitdelay&quot; &gt;One Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://timmingerror&quot; &gt;Timming Error&lt;/a&gt;.&lt;br&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string MessageTimeOut {
            get {
                return ResourceManager.GetString("MessageTimeOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;One Bit Delay Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://zerobitdelay&quot; &gt;Zero Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://messagetimeout&quot; &gt;Message Time out&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://timmingerror&quot; &gt;Timming Error&lt;/a&gt;.&lt;br&gt;
        ///
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string OneBitDelay {
            get {
                return ResourceManager.GetString("OneBitDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Run only on the first Core Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string RunOneCore {
            get {
                return ResourceManager.GetString("RunOneCore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Shared Cpu Usage&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string SharedCpuUsage {
            get {
                return ResourceManager.GetString("SharedCpuUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Shared File Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string SharedFile {
            get {
                return ResourceManager.GetString("SharedFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Silent Interval Help&lt;/h3&gt;&lt;br&gt;
        ///This is a method to solve Synchronization Problem.
        ///We enhance the scheme by introducing silent in-
        ///tervals between frames. During a silent interval no packet
        ///transfer occurs between sender and receiver. We assume
        ///that the parties have previously determined the length of
        ///the silent interval. This interval can either be a default value
        ///or the covert channel itself can be initially used to send this
        ///value before the actual data transfer  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string SilentInterval {
            get {
                return ResourceManager.GetString("SilentInterval", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;midle Start Of Frame Help&lt;/h3&gt;&lt;br&gt;
        ///This is a method to solve Synchronization Problem.
        ///As a precaution against low levels of jitter in the network,
        ///each packet is sent in the middle of the timing interval.
        ///Moreover, upon receipt of every SOF packet, the receiver
        ///aligns itself with the newly received SOF by assuming that
        ///the SOF arrived exactly in the middle of the timing interval.
        ///This aligns the sender and receiver timing windows and in
        ///turn helps maintain synchro [rest of string was truncated]&quot;;.
        /// </summary>
        public static string StartOfFrame {
            get {
                return ResourceManager.GetString("StartOfFrame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;TCP Packet Delay Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;table&gt;
        ///&lt;tr&gt;
        ///&lt;td align=&apos;center&apos; colspan=3&gt;Recommended Delays&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;tr&gt;
        ///&lt;td&gt;&lt;/td&gt;
        ///&lt;td&gt;Local Network&lt;/td&gt;
        ///&lt;td&gt;Internet&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;tr&gt;
        ///
        ///&lt;td&gt;Zero Bit Delay&lt;/td&gt;
        ///&lt;td&gt;0&lt;/td&gt;
        ///&lt;td&gt;0&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;tr&gt;
        ///&lt;td&gt;One Bit Delay&lt;/td&gt;
        ///&lt;td&gt;100&lt;/td&gt;
        ///&lt;td&gt;3000&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;tr&gt;
        ///&lt;td&gt;Message Timeout&lt;/td&gt;
        ///&lt;td&gt;200&lt;/td&gt;
        ///&lt;td&gt;6000&lt;/td&gt;
        ///&lt;/tr&gt;
        ///&lt;/table&gt;
        ///If some values of the above table reduce, the transmition may occure some erorrs.
        ///Its  als [rest of string was truncated]&quot;;.
        /// </summary>
        public static string TcpPacketDelay {
            get {
                return ResourceManager.GetString("TcpPacketDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Timming Error Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://zerobitdelay&quot; &gt;Zero Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://onebitdelay&quot; &gt;One Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://messagetimeout&quot; &gt;Message Time out&lt;/a&gt;.&lt;br&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string TimmingError {
            get {
                return ResourceManager.GetString("TimmingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Upnp Help&lt;/h3&gt;&lt;br&gt;
        ///Universal Plug and Play is a protocol witch is 
        /// used to communicate with modem/router and auto-create 
        /// port forwarding for the appropriate port. With that 
        /// way when a PC speak to this port on   router the communication 
        /// transmitted to the local PC in the same port.
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string Upnp {
            get {
                return ResourceManager.GetString("Upnp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.0.5.
        /// </summary>
        public static string version {
            get {
                return ResourceManager.GetString("version", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;body bgcolor=&quot;FFFFCC&quot;&gt;&lt;/body&gt;
        ///&lt;h3&gt;Zero Bit Delay Help&lt;/h3&gt;&lt;br&gt;
        ///
        ///&lt;h4&gt;Related Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://onebitdelay&quot; &gt;One Bit Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://messagetimeout&quot; &gt;Message Time out&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://timmingerror&quot; &gt;Timming Error&lt;/a&gt;.&lt;br&gt;
        ///&lt;h4&gt;Other Help Links:&lt;h4&gt;
        ///&lt;a href = &quot;res://TcpPacketDelay&quot; &gt;TCP Packet Delay&lt;/a&gt;.&lt;br&gt;
        ///&lt;a href = &quot;res://main&quot; &gt;Covert Channel General&lt;/a&gt;&lt;br&gt;.
        /// </summary>
        public static string ZeroBitDelay {
            get {
                return ResourceManager.GetString("ZeroBitDelay", resourceCulture);
            }
        }
    }
}