using System;
using System.Runtime.InteropServices;
using UnityHelper.Extensions;


namespace UnityHelper.Unsafe
{
    /// <summary>
    /// Encapsulates a pointer to unmanaged memory.
    /// Provides methods to safely access and modify unmanaged memory.
    /// </summary>
    /// <typeparam name="T">The type of the struct to be stored in unmanaged memory.</typeparam>
    public unsafe struct NativeObject<T> : IDisposable where T : struct
    {
        private IntPtr ptr;

        internal NativeObject(nint ptr)
        {
            this.ptr = ptr;
        }

        public readonly IntPtr GetPointer()
        {
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Pointer is null or memory has been freed.");
            }
            return ptr;
        }

        /// <summary>
        /// Gets the value stored in the unmanaged memory.
        /// </summary>
        public readonly ref T GetValue()
        {
            IntPtr pointer = GetPointer(); // Ensure pointer is valid

            // Cast the pointer to a reference to type T
            ref T refToValue = ref System.Runtime.CompilerServices.Unsafe.AsRef<T>((void*)pointer);
            return ref refToValue;
        }

        internal void Invalidate()
        {
            ptr = IntPtr.Zero;
        }

        public void Dispose()
        {
            UnmanagedObject.Free(ref this);
        }
    }

    /// <summary>
    /// Provides methods to allocate and free unmanaged memory.
    /// </summary>
    public static class UnmanagedObject
    {
        /// <summary>
        /// Allocates unmanaged memory for a struct of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the struct to allocate memory for.</typeparam>
        /// <returns>An <see cref="NativeObject{T}"/> that encapsulates the pointer to the unmanaged memory.</returns>
        /// <exception cref="OutOfMemoryException">Thrown if memory allocation fails.</exception>
        public unsafe static NativeObject<T> Allocate<T>() where T : struct
        {
            // Calculate the size of the struct
            int size = Marshal.SizeOf<T>();

            // Allocate unmanaged memory
            nint ptr = Marshal.AllocHGlobal(size);

            if (ptr == IntPtr.Zero)
            {
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");
            }

            return new NativeObject<T>(ptr);
        }

        /// <summary>
        /// Frees the unmanaged memory allocated for the specified <see cref="NativeObject{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the struct stored in the unmanaged memory.</typeparam>
        /// <param name="unmanagedObject">The <see cref="NativeObject{T}"/> to free.</param>
        /// <exception cref="InvalidOperationException">Thrown if the pointer is null or the memory has already been freed.</exception>
        public static void Free<T>(ref NativeObject<T> unmanagedObject) where T : struct
        {
            // Free the unmanaged memory
            IntPtr pointer = unmanagedObject.GetPointer(); // Ensure pointer is valid
            Marshal.FreeHGlobal(pointer);
            unmanagedObject.Invalidate();
        }
    }
}
