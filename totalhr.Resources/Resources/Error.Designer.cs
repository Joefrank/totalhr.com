﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace totalhr.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Error {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Error() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("totalhr.Resources.Error", typeof(Error).Assembly);
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
        ///   Looks up a localized string similar to Access Denied.
        /// </summary>
        public static string Error_Access_Denied {
            get {
                return ResourceManager.GetString("Error_Access_Denied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but you cannot strip user of all profiles. A user must have some profiles..
        /// </summary>
        public static string Error_Cant_Remove_All_Profiles {
            get {
                return ResourceManager.GetString("Error_Cant_Remove_All_Profiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but you cannot remove all roles from user. User must have at least one role..
        /// </summary>
        public static string Error_Cant_Remove_All_Roles {
            get {
                return ResourceManager.GetString("Error_Cant_Remove_All_Roles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but could not create user..
        /// </summary>
        public static string Error_Could_Not_CreateUser {
            get {
                return ResourceManager.GetString("Error_Could_Not_CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must select a department for the new user!.
        /// </summary>
        public static string Error_Missing_Department {
            get {
                return ResourceManager.GetString("Error_Missing_Department", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but no user found!.
        /// </summary>
        public static string Error_No_User_Found {
            get {
                return ResourceManager.GetString("Error_No_User_Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but no user has this role!.
        /// </summary>
        public static string Error_No_User_Under_Role {
            get {
                return ResourceManager.GetString("Error_No_User_Under_Role", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You do not have the right to edit this user profile..
        /// </summary>
        public static string Error_NoAccess_ToUserProfile {
            get {
                return ResourceManager.GetString("Error_NoAccess_ToUserProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You do not have the right to edit this user profile..
        /// </summary>
        public static string Error_NoAccess_ToUserRole {
            get {
                return ResourceManager.GetString("Error_NoAccess_ToUserRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry but the user you searched for cannot be found..
        /// </summary>
        public static string Error_Profile_NoUserDetails {
            get {
                return ResourceManager.GetString("Error_Profile_NoUserDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry could not save user profiles!.
        /// </summary>
        public static string MSG_Error_Could_Not_Save_PRofiles {
            get {
                return ResourceManager.GetString("MSG_Error_Could_Not_Save_PRofiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry could not save user roles!.
        /// </summary>
        public static string MSG_Error_Could_Not_Save_Roles {
            get {
                return ResourceManager.GetString("MSG_Error_Could_Not_Save_Roles", resourceCulture);
            }
        }
    }
}
