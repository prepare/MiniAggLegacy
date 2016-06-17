#region License
// Copyright (c) 2013 Antonie Blom
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#if USE_GLFW3
using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Pencil.Gaming {
	internal static unsafe class Glfw64 {
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwInit();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwTerminate();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetVersion(out int major, out int minor, out int rev);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern sbyte * glfwGetVersionString();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwErrorFun glfwSetErrorCallback(GlfwErrorFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwMonitorPtr * glfwGetMonitors(out int count);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwMonitorPtr glfwGetPrimaryMonitor();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetMonitorPos(GlfwMonitorPtr monitor, out int xpos, out int ypos);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetMonitorPhysicalSize(GlfwMonitorPtr monitor, out int width, out int height);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern sbyte * glfwGetMonitorName(GlfwMonitorPtr monitor);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwVidMode * glfwGetVideoModes(GlfwMonitorPtr monitor, out int count);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwVidMode* glfwGetVideoMode(GlfwMonitorPtr monitor);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetGamma(GlfwMonitorPtr monitor, float gamma);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetGammaRamp(GlfwMonitorPtr monitor, out GlfwGammaRampInternal ramp);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetGammaRamp(GlfwMonitorPtr monitor, ref GlfwGammaRamp ramp);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwDefaultWindowHints();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwWindowHint(WindowHint target, int hint);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwWindowPtr glfwCreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, GlfwMonitorPtr monitor, GlfwWindowPtr share);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwDestroyWindow(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwWindowShouldClose(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetWindowShouldClose(GlfwWindowPtr window, int value);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetWindowTitle(GlfwWindowPtr window, [MarshalAs(UnmanagedType.LPStr)] string title);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetWindowPos(GlfwWindowPtr window, out int xpos, out int ypos);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetWindowPos(GlfwWindowPtr window, int xpos, int ypos);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern void glfwGetWindowSize(GlfwWindowPtr window, out int width, out int height);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetWindowSize(GlfwWindowPtr window, int width, int height);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwIconifyWindow(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwRestoreWindow(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwShowWindow(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwHideWindow(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwMonitorPtr glfwGetWindowMonitor(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwGetWindowAttrib(GlfwWindowPtr window, int param);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetWindowUserPointer(GlfwWindowPtr window, IntPtr pointer);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern IntPtr glfwGetWindowUserPointer(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowPosFun glfwSetWindowPosCallback(GlfwWindowPtr window, GlfwWindowPosFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowSizeFun glfwSetWindowSizeCallback(GlfwWindowPtr window, GlfwWindowSizeFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowCloseFun glfwSetWindowCloseCallback(GlfwWindowPtr window, GlfwWindowCloseFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowRefreshFun glfwSetWindowRefreshCallback(GlfwWindowPtr window, GlfwWindowRefreshFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowFocusFun glfwSetWindowFocusCallback(GlfwWindowPtr window, GlfwWindowFocusFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwWindowIconifyFun glfwSetWindowIconifyCallback(GlfwWindowPtr window, GlfwWindowIconifyFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwPollEvents();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwWaitEvents();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwGetInputMode(GlfwWindowPtr window, InputMode mode);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetInputMode(GlfwWindowPtr window, InputMode mode, CursorMode value);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwGetKey(GlfwWindowPtr window, Key key);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwGetMouseButton(GlfwWindowPtr window, MouseButton button);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwGetCursorPos(GlfwWindowPtr window, out double xpos, out double ypos);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetCursorPos(GlfwWindowPtr window, double xpos, double ypos);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwKeyFun glfwSetKeyCallback(GlfwWindowPtr window, GlfwKeyFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwCharFun glfwSetCharCallback(GlfwWindowPtr window, GlfwCharFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwMouseButtonFun glfwSetMouseButtonCallback(GlfwWindowPtr window, GlfwMouseButtonFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwCursorPosFun glfwSetCursorPosCallback(GlfwWindowPtr window, GlfwCursorPosFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwCursorEnterFun glfwSetCursorEnterCallback(GlfwWindowPtr window, GlfwCursorEnterFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwScrollFun glfwSetScrollCallback(GlfwWindowPtr window, GlfwScrollFun cbfun);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwJoystickPresent(Joystick joy);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern float * glfwGetJoystickAxes(Joystick joy, out int numaxes);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern byte * glfwGetJoystickButtons(Joystick joy, out int numbuttons);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern sbyte * glfwGetJoystickName(Joystick joy);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetClipboardString(GlfwWindowPtr window, [MarshalAs(UnmanagedType.LPStr)] string @string);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern sbyte * glfwGetClipboardString(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern double glfwGetTime();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSetTime(double time);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwMakeContextCurrent(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern GlfwWindowPtr glfwGetCurrentContext();
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSwapBuffers(GlfwWindowPtr window);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern void glfwSwapInterval(int interval);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern int glfwExtensionSupported([MarshalAs(UnmanagedType.LPStr)] string extension);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity] 
		internal static extern IntPtr glfwGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string procname);

		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern void glfwGetFramebufferSize(GlfwWindowPtr window, out int width, out int height);
		[DllImport("natives64/glfw.dll"), SuppressUnmanagedCodeSecurity]
		internal static extern GlfwFramebufferSizeFun glfwSetFramebufferSizeCallback(GlfwWindowPtr window, GlfwFramebufferSizeFun cbfun);
	}
}

#endif