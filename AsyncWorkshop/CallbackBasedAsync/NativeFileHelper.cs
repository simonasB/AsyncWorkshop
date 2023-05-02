using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CallbackBasedAsync;

public class NativeFileHelper
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool CloseHandle(IntPtr hObject);

    public static void WriteAsyncUsingCallback(string filePath, byte[] data, Action<bool> callback)
    {
        WriteBytesToFile(filePath, data, callback);
    }

    public static CustomTask WriteAsyncUsingCustomTask(string filePath, byte[] data)
    {
        var task = new CustomTask();

        // Implement method using CustomTask based approach
        // You will need to call WriteBytesToFile, use SetResult(), SetException()

        return task;
    }
    
    private static unsafe void WriteBytesToFile(string filePath, byte[] data, Action<bool> callback)
    {
        const uint GENERIC_WRITE = 0x40000000;
        const uint OPEN_ALWAYS = 4;
        const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        const uint ERROR_IO_PENDING = 997;

        SafeFileHandle hFile = CreateFile(
            filePath,
            GENERIC_WRITE,
            0,
            IntPtr.Zero,
            OPEN_ALWAYS,
            FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
            IntPtr.Zero
        );

        ThreadPool.BindHandle(hFile);
        
        if (!hFile.IsInvalid)
        {
            Overlapped overlapped = new Overlapped();
            NativeOverlapped* pOverlapped = overlapped.Pack(FileIoCompletionRoutine, data);
            uint bytesWritten;

            bool result = WriteFile(hFile, data, (uint)data.Length, out bytesWritten, (nint)pOverlapped);

            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                if (error != ERROR_IO_PENDING)
                {
                    FileIoCompletionRoutine(0, 0, pOverlapped);
                }
            }
        }
        else
        {
            callback(false);
        }

        void FileIoCompletionRoutine(uint errorCode, uint numBytes, NativeOverlapped* pNativeOverlapped)
        {
            Overlapped.Unpack(pNativeOverlapped);
            Overlapped.Free(pNativeOverlapped);
            CloseHandle(hFile.DangerousGetHandle());
            callback(errorCode == 0);
        }
    }
}