﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectDependencyGraph.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProjectDependencyGraph.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The digraph code has been copied to your clipboard..
        /// </summary>
        internal static string DigraphCopiedToClipboard {
            get {
                return ResourceManager.GetString("DigraphCopiedToClipboard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GraphViz couldn&apos;t be found on your machine. The digraph code has been copied to your clipboard. You may go to http://graphviz-dev.appspot.com/ to render it there..
        /// </summary>
        internal static string DigraphCopiedToClipboardByGraphVizNotFound {
            get {
                return ResourceManager.GetString("DigraphCopiedToClipboardByGraphVizNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select the folder containing &apos;dotty.exe&apos; and &apos;dot.exe&apos;. Usually in your Program Files under GraphViz bin..
        /// </summary>
        internal static string LocateGraphViz {
            get {
                return ResourceManager.GetString("LocateGraphViz", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GraphViz wasn&apos;t found. Would you like to locate it?.
        /// </summary>
        internal static string LocateGraphVizQuetion {
            get {
                return ResourceManager.GetString("LocateGraphVizQuetion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dependency Graph.
        /// </summary>
        internal static string RenderGraphMessageBoxTitle {
            get {
                return ResourceManager.GetString("RenderGraphMessageBoxTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Render the simplified version skipping implied dependencies?.
        /// </summary>
        internal static string RenderSimplifiedDependencyGraphQuestion {
            get {
                return ResourceManager.GetString("RenderSimplifiedDependencyGraphQuestion", resourceCulture);
            }
        }
    }
}
