namespace RadicalResearch.Net.Dns
{
    using System;

    [Flags]
    public enum MessageFlags : ushort
    {
        Response = 0x8000,
        OpCodeQuery = 0x0,
        OpCodeIQuery = 0x800,
        OpCodeStatus = 0x1000,
        AuthoritativeAnswer = 0x400,
        Truncation = 0x200,
        RecursionDesired = 0x100,
        RecursionAvailable = 0x80,
        Authenticated = 0x20,
        AcceptNonAuthenticated = 0x10,
        RcodeFormatError = 0x1,
        RcodeServerFailure = 0x2,
        RcodeNameError = 0x3,
        RcodeNotImplemented = 0x4,
        RcodeRefused = 0x5
    }
}