using System.Runtime.InteropServices;
using System.Text;
using ObsInterop;

namespace DotnetObsPluginWithNativeAOT;

public static class ObsPlugin
{
    public static nint ObsModulePointer { get; set; }

    [UnmanagedCallersOnly(EntryPoint = "obs_module_set_pointer", CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static void SetPointer(nint obsModulePointer)
    {
        Log("[blog] Pointer Saved!");
        ObsModulePointer = obsModulePointer;
    }
    
    [UnmanagedCallersOnly(EntryPoint = "obs_module_ver", CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static uint GetVersion()
    {
        Log("[blog] Returned version!");
        
        var major = (uint) Obs.Version.Major;
        var minor = (uint) Obs.Version.Minor;
        var patch = (uint) Obs.Version.Build;
        var version = (major << 24) | (minor << 16) | patch;
        return version;
    }

    [UnmanagedCallersOnly(EntryPoint = "obs_module_load", CallConvs = new[] {typeof(System.Runtime.CompilerServices.CallConvCdecl)})]
    public static bool ModuleLoad()
    {
        Log("[blog] Loaded!");
        return true;
    }

    private static unsafe void Log(string text)
    {
        var asciiBytes = Encoding.UTF8.GetBytes(text);
        fixed (byte* logMessagePtr = asciiBytes)
        {
            ObsBase.blogva(ObsBase.LOG_INFO, (sbyte*) logMessagePtr, null);   
        }
    }
}