﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{

    public enum Keys
    {
        Escape = 0x01,
        D1 = 0x02,
        D2 = 0x03,
        D3 = 0x04,
        D4 = 0x05,
        D5 = 0x06,
        D6 = 0x07,
        D7 = 0x08,
        D8 = 0x09,
        D9 = 0x0A,
        D0 = 0x0B,
        Minus = 0x0C,   /* - on main keyboard */
        EQUALS = 0x0D,
        Back = 0x0E,   /* backspace */
        Tab = 0x0F,
        Q = 0x10,
        W = 0x11,
        E = 0x12,
        R = 0x13,
        T = 0x14,
        Y = 0x15,
        U = 0x16,
        I = 0x17,
        O = 0x18,
        P = 0x19,
        Lbracket = 0x1A,
        Rbracket = 0x1B,
        Return = 0x1C,   /* Enter on main keyboard */
        Lcontrol = 0x1D,
        A = 0x1E,
        S = 0x1F,
        D = 0x20,
        F = 0x21,
        G = 0x22,
        H = 0x23,
        J = 0x24,
        K = 0x25,
        L = 0x26,
        Semicolon = 0x27,
        Apostrophe = 0x28,
        Grave = 0x29,   /* accent grave */
        Lshift = 0x2A,
        Backslash = 0x2B,
        Z = 0x2C,
        X = 0x2D,
        C = 0x2E,
        V = 0x2F,
        B = 0x30,
        N = 0x31,
        M = 0x32,
        Comma = 0x33,
        Period = 0x34,   /* . on main keyboard */
        Slash = 0x35,  /* / on main keyboard */
        Rshift = 0x36,
        Multiply = 0x37,   /* * on numeric keypad */
        Lmenu = 0x38,  /* left Alt */
        Space = 0x39,
        Capital = 0x3A,
        F1 = 0x3B,
        F2 = 0x3C,
        F3 = 0x3D,
        F4 = 0x3E,
        F5 = 0x3F,
        F6 = 0x40,
        F7 = 0x41,
        F8 = 0x42,
        F9 = 0x43,
        F10 = 0x44,
        Numlock = 0x45,
        Scroll = 0x46,   /* Scroll Lock */
        Numpad7 = 0x47,
        Numpad8 = 0x48,
        Numpad9 = 0x49,
        Subtract = 0x4A,   /* - on numeric keypad */
        Numpad4 = 0x4B,
        Numpad5 = 0x4C,
        Numpad6 = 0x4D,
        Add = 0x4E,   /* + on numeric keypad */
        Numpad1 = 0x4F,
        Numpad2 = 0x50,
        Numpad3 = 0x51,
        Numpad0 = 0x52,
        Decimal = 0x53,   /* . on numeric keypad */
        Oem102 = 0x56,  /* <> or \| on RT 102-key keyboard (Non-U.S.) */
        F11 = 0x57,
        F12 = 0x58,
        F13 = 0x64,   /*                     (NEC PC98) */
        F14 = 0x65,  /*                     (NEC PC98) */
        F15 = 0x66,  /*                     (NEC PC98) */
        Kana = 0x70, /* (Japanese keyboard)            */
        AbntC1 = 0x73,/* /? on Brazilian keyboard */
        Convert = 0x79,    /* (Japanese keyboard)            */
        Noconvert = 0x7B,    /*, (Japanese keyboard)            */
        Yen = 0x7D,    /* (Japanese keyboard)            */
        AbntC2 = 0x7E,   /* Numpad . on Brazilian keyboard */
        Numpadequals = 0x8D,  /* = on numeric keypad (NEC PC98) */
        Prevtrack = 0x90,  /* Previous Track (CIRCUMFLEX on Japanese keyboard) */
        At = 0x91,    /*                     (NEC PC98) */
        Colon = 0x92,   /*                     (NEC PC98) */
        Underline = 0x93,  /*                     (NEC PC98) */
        Kanji = 0x94, /* (Japanese keyboard)            */
        Stop = 0x95,   /*                     (NEC PC98) */
        Ax = 0x96,  /*                     (Japan AX) */
        Unlabeled = 0x97, /*                        (J3100) */
        Nexttrack = 0x99,/* Next Track */
        Numpadenter = 0x9C,    /* Enter on numeric keypad */
        Rcontrol = 0x9D,
        Mute = 0xA0,   /* Mute */
        Calculator = 0xA1,  /* Calculator */
        Playpause = 0xA2,    /* Play / Pause */
        Mediastop = 0xA4,    /* Media Stop */
        Volumedown = 0xAE,    /* Volume - */
        Volumeup = 0xB0,    /* Volume + */
        Webhome = 0xB2,    /* Web home */
        Numpadcomma = 0xB3,    /* , on numeric keypad (NEC PC98) */
        Divide = 0xB5,    /* / on numeric keypad */
        Sysrq = 0xB7,
        Rmenu = 0xB8,    /* right Alt */
        Pause = 0xC5,    /* Pause */
        Home = 0xC7,   /* Home on arrow keypad */
        Up = 0xC8,    /* UpArrow on arrow keypad */
        Prior = 0xC9,   /* PgUp on arrow keypad */
        Left = 0xCB,  /* LeftArrow on arrow keypad */
        Right = 0xCD, /* RightArrow on arrow keypad */
        End = 0xCF,    /* End on arrow keypad */
        Down = 0xD0,   /* DownArrow on arrow keypad */
        Next = 0xD1,  /* PgDn on arrow keypad */
        Insert = 0xD2, /* Insert on arrow keypad */
        Delete = 0xD3,   /* Delete on arrow keypad */
        Lwin = 0xDB,  /* Left Windows key */
        Rwin = 0xDC, /* Right Windows key */
        Apps = 0xDD,/* AppMenu key */
        Power = 0xDE,   /* System Power */
        Sleep = 0xDF,   /* System Sleep */
        Wake = 0xE3,  /* System Wake */
        Websearch = 0xE5, /* Web Search */
        Webfavorites = 0xE6,/* Web Favorites */
        Webrefresh = 0xE7,   /* Web Refresh */
        Webstop = 0xE8,  /* Web Stop */
        Webforward = 0xE9, /* Web Forward */
        Webback = 0xEA,  /* Web Back */
        Mycomputer = 0xEB,/* My Computer */
        Mail = 0xEC,   /* Mail */
        Mediaselect = 0xED,   /* Media Select */

        /*
         *  Alternate names for keys, to facilitate transition from DOS.
         */
        Backspace = Back,            /* backspace */
        Numpadstar = Multiply,    /* * on numeric keypad */
        Lalt = Lmenu,      /* left Alt */
        Capslock = Capital,     /* CapsLock */
        Numpadminus = Subtract,     /* - on numeric keypad */
        Numpadplus = Add,     /* + on numeric keypad */
        Numpadperiod = Decimal,     /* . on numeric keypad */
        Numpadslash = Divide,     /* / on numeric keypad */
        Ralt = Rmenu,    /* right Alt */
        Uparrow = Up,     /* UpArrow on arrow keypad */
        Pgup = Prior,     /* PgUp on arrow keypad */
        Leftarrow = Left,     /* LeftArrow on arrow keypad */
        Rightarrow = Right,     /* RightArrow on arrow keypad */
        Downarrow = Down,    /* DownArrow on arrow keypad */
        Pgdn = Next,    /* PgDn on arrow keypad */

        /*
         *  Alternate names for keys originally not used on US keyboards.
         */
        Circumflex = Prevtrack       /* Japanese keyboard */
    }

    // Summary:
    //     Defines possible keyboard key codes.
    //public enum Keys:int
    //{
    //    // Summary:
    //    //     The number 0.
    //    D0 = 0,
    //    //
    //    // Summary:
    //    //     The number 1.
    //    D1 = 1,
    //    //
    //    // Summary:
    //    //     The number 2.
    //    D2 = 2,
    //    //
    //    // Summary:
    //    //     The number 3.
    //    D3 = 3,
    //    //
    //    // Summary:
    //    //     The number 4.
    //    D4 = 4,
    //    //
    //    // Summary:
    //    //     The number 5.
    //    D5 = 5,
    //    //
    //    // Summary:
    //    //     The number 6.
    //    D6 = 6,
    //    //
    //    // Summary:
    //    //     The number 7.
    //    D7 = 7,
    //    //
    //    // Summary:
    //    //     The number 8.
    //    D8 = 8,
    //    //
    //    // Summary:
    //    //     The number 9.
    //    D9 = 9,
    //    //
    //    // Summary:
    //    //     The letter A.
    //    A = 10,
    //    //
    //    // Summary:
    //    //     The letter B.
    //    B = 11,
    //    //
    //    // Summary:
    //    //     The letter C.
    //    C = 12,
    //    //
    //    // Summary:
    //    //     The letter D.
    //    D = 13,
    //    //
    //    // Summary:
    //    //     The letter E.
    //    E = 14,
    //    //
    //    // Summary:
    //    //     The letter F.
    //    F = 15,
    //    //
    //    // Summary:
    //    //     The letter G.
    //    G = 16,
    //    //
    //    // Summary:
    //    //     The letter H.
    //    H = 17,
    //    //
    //    // Summary:
    //    //     The letter I.
    //    I = 18,
    //    //
    //    // Summary:
    //    //     The letter J.
    //    J = 19,
    //    //
    //    // Summary:
    //    //     The letter K.
    //    K = 20,
    //    //
    //    // Summary:
    //    //     The letter L.
    //    L = 21,
    //    //
    //    // Summary:
    //    //     The letter M.
    //    M = 22,
    //    //
    //    // Summary:
    //    //     The letter N.
    //    N = 23,
    //    //
    //    // Summary:
    //    //     The letter O.
    //    O = 24,
    //    //
    //    // Summary:
    //    //     The letter P.
    //    P = 25,
    //    //
    //    // Summary:
    //    //     The letter Q.
    //    Q = 26,
    //    //
    //    // Summary:
    //    //     The letter R.
    //    R = 27,
    //    //
    //    // Summary:
    //    //     The letter S.
    //    S = 28,
    //    //
    //    // Summary:
    //    //     The letter T.
    //    T = 29,
    //    //
    //    // Summary:
    //    //     The letter U.
    //    U = 30,
    //    //
    //    // Summary:
    //    //     The letter V.
    //    V = 31,
    //    //
    //    // Summary:
    //    //     The letter W.
    //    W = 32,
    //    //
    //    // Summary:
    //    //     The letter X.
    //    X = 33,
    //    //
    //    // Summary:
    //    //     The letter Y.
    //    Y = 34,
    //    //
    //    // Summary:
    //    //     The letter Z.
    //    Z = 35,
    //    //
    //    // Summary:
    //    //     The AbntC1 key on Brazillian keyboards.
    //    AbntC1 = 36,
    //    //
    //    // Summary:
    //    //     The AbntC2 key on Brazillian keyboards.
    //    AbntC2 = 37,
    //    //
    //    // Summary:
    //    //     The apostrophe key.
    //    Apostrophe = 38,
    //    //
    //    // Summary:
    //    //     The Applications key.
    //    Applications = 39,
    //    //
    //    // Summary:
    //    //     The Japanese At key.
    //    AT = 40,
    //    //
    //    // Summary:
    //    //     The Japanese Ax key.
    //    AX = 41,
    //    //
    //    // Summary:
    //    //     The Backspace.
    //    Backspace = 42,
    //    //
    //    // Summary:
    //    //     The back slash key.
    //    Backslash = 43,
    //    //
    //    // Summary:
    //    //     The calculator key.
    //    Calculator = 44,
    //    //
    //    // Summary:
    //    //     The Caps Lock key.
    //    CapsLock = 45,
    //    //
    //    // Summary:
    //    //     The colon key.
    //    Colon = 46,
    //    //
    //    // Summary:
    //    //     The comma key.
    //    Comma = 47,
    //    //
    //    // Summary:
    //    //     The Japanese Convert key.
    //    Convert = 48,
    //    //
    //    // Summary:
    //    //     The Delete key.
    //    Delete = 49,
    //    //
    //    // Summary:
    //    //     The Down Arrow key.
    //    DownArrow = 50,
    //    //
    //    // Summary:
    //    //     The End key.
    //    End = 51,
    //    //
    //    // Summary:
    //    //     The equals key.
    //    Equals = 52,
    //    //
    //    // Summary:
    //    //     The Escape key.
    //    Escape = 53,
    //    //
    //    // Summary:
    //    //     The F1 key.
    //    F1 = 54,
    //    //
    //    // Summary:
    //    //     The F2 key.
    //    F2 = 55,
    //    //
    //    // Summary:
    //    //     The F3 key.
    //    F3 = 56,
    //    //
    //    // Summary:
    //    //     The F4 key.
    //    F4 = 57,
    //    //
    //    // Summary:
    //    //     The F5 key.
    //    F5 = 58,
    //    //
    //    // Summary:
    //    //     The F6 key.
    //    F6 = 59,
    //    //
    //    // Summary:
    //    //     The F7 key.
    //    F7 = 60,
    //    //
    //    // Summary:
    //    //     The F8 key.
    //    F8 = 61,
    //    //
    //    // Summary:
    //    //     The F9 key.
    //    F9 = 62,
    //    //
    //    // Summary:
    //    //     The F10 key.
    //    F10 = 63,
    //    //
    //    // Summary:
    //    //     The F11 key.
    //    F11 = 64,
    //    //
    //    // Summary:
    //    //     The F12 key.
    //    F12 = 65,
    //    //
    //    // Summary:
    //    //     The F13 key.
    //    F13 = 66,
    //    //
    //    // Summary:
    //    //     The F14 key.
    //    F14 = 67,
    //    //
    //    // Summary:
    //    //     The F15 key.
    //    F15 = 68,
    //    //
    //    // Summary:
    //    //     The grav accent (`) key.
    //    Grave = 69,
    //    //
    //    // Summary:
    //    //     The Home key.
    //    Home = 70,
    //    //
    //    // Summary:
    //    //     The Insert key.
    //    Insert = 71,
    //    //
    //    // Summary:
    //    //     The Japanese Kana key.
    //    Kana = 72,
    //    //
    //    // Summary:
    //    //     The Japanese Kanji key.
    //    Kanji = 73,
    //    //
    //    // Summary:
    //    //     The left square bracket key.
    //    LeftBracket = 74,
    //    //
    //    // Summary:
    //    //     The left Ctrl key.
    //    LeftControl = 75,
    //    //
    //    // Summary:
    //    //     The Left Arrow key.
    //    LeftArrow = 76,
    //    //
    //    // Summary:
    //    //     The left Alt key.
    //    LeftAlt = 77,
    //    //
    //    // Summary:
    //    //     The left Shift key.
    //    LeftShift = 78,
    //    //
    //    // Summary:
    //    //     The left Windows key.
    //    LeftWindowsKey = 79,
    //    //
    //    // Summary:
    //    //     The Mail key.
    //    Mail = 80,
    //    //
    //    // Summary:
    //    //     The Media Select key.
    //    MediaSelect = 81,
    //    //
    //    // Summary:
    //    //     The Media Stop key.
    //    MediaStop = 82,
    //    //
    //    // Summary:
    //    //     The minus key.
    //    Minus = 83,
    //    //
    //    // Summary:
    //    //     The Mute key.
    //    Mute = 84,
    //    //
    //    // Summary:
    //    //     The My Computer key.
    //    MyComputer = 85,
    //    //
    //    // Summary:
    //    //     The Next Track key.
    //    NextTrack = 86,
    //    //
    //    // Summary:
    //    //     The Japanese No Convert key.
    //    NoConvert = 87,
    //    //
    //    // Summary:
    //    //     The NumberLock key.
    //    NumberLock = 88,
    //    //
    //    // Summary:
    //    //     The number 0 on the NumberPad.
    //    NumberPad0 = 89,
    //    //
    //    // Summary:
    //    //     The number 1 on the NumberPad.
    //    NumberPad1 = 90,
    //    //
    //    // Summary:
    //    //     The number 2 on the NumberPad.
    //    NumberPad2 = 91,
    //    //
    //    // Summary:
    //    //     The number 3 on the NumberPad.
    //    NumberPad3 = 92,
    //    //
    //    // Summary:
    //    //     The number 4 on the NumberPad.
    //    NumberPad4 = 93,
    //    //
    //    // Summary:
    //    //     The number 5 on the NumberPad.
    //    NumberPad5 = 94,
    //    //
    //    // Summary:
    //    //     The number 6 on the NumberPad.
    //    NumberPad6 = 95,
    //    //
    //    // Summary:
    //    //     The number 7 on the NumberPad.
    //    NumberPad7 = 96,
    //    //
    //    // Summary:
    //    //     The number 8 on the NumberPad.
    //    NumberPad8 = 97,
    //    //
    //    // Summary:
    //    //     The number 9 on the NumberPad.
    //    NumberPad9 = 98,
    //    //
    //    // Summary:
    //    //     The comma key on the NumberPad.
    //    NumberPadComma = 99,
    //    //
    //    // Summary:
    //    //     The Enter key on the NumberPad.
    //    NumberPadEnter = 100,
    //    //
    //    // Summary:
    //    //     The equals key on the NumberPad.
    //    NumberPadEquals = 101,
    //    //
    //    // Summary:
    //    //     The minus key on the NumberPad.
    //    NumberPadMinus = 102,
    //    //
    //    // Summary:
    //    //     The period key on the NumberPad.
    //    NumberPadPeriod = 103,
    //    //
    //    // Summary:
    //    //     The plus key on the NumberPad.
    //    NumberPadPlus = 104,
    //    //
    //    // Summary:
    //    //     The slash key on the NumberPad.
    //    NumberPadSlash = 105,
    //    //
    //    // Summary:
    //    //     The asterisk key on the NumberPad.
    //    NumberPadStar = 106,
    //    //
    //    // Summary:
    //    //     The British and German OEM102 key.
    //    Oem102 = 107,
    //    //
    //    // Summary:
    //    //     The Page Down key.
    //    PageDown = 108,
    //    //
    //    // Summary:
    //    //     The Page Up key.
    //    PageUp = 109,
    //    //
    //    // Summary:
    //    //     The Pause key.
    //    Pause = 110,
    //    //
    //    // Summary:
    //    //     The period key.
    //    Period = 111,
    //    //
    //    // Summary:
    //    //     The Play/Pause key.
    //    PlayPause = 112,
    //    //
    //    // Summary:
    //    //     The Power key.
    //    Power = 113,
    //    //
    //    // Summary:
    //    //     The Previous Track key.
    //    PreviousTrack = 114,
    //    //
    //    // Summary:
    //    //     The right square bracket key.
    //    RightBracket = 115,
    //    //
    //    // Summary:
    //    //     The right Ctrl key.
    //    RightControl = 116,
    //    //
    //    // Summary:
    //    //     The Return/Enter key.
    //    Return = 117,
    //    //
    //    // Summary:
    //    //     The Right Arrow key.
    //    RightArrow = 118,
    //    //
    //    // Summary:
    //    //     The right Alt key.
    //    RightAlt = 119,
    //    //
    //    // Summary:
    //    //     The right Shift key.
    //    RightShift = 120,
    //    //
    //    // Summary:
    //    //     The right Windows key.
    //    RightWindowsKey = 121,
    //    //
    //    // Summary:
    //    //     The Scroll Lock key.
    //    ScrollLock = 122,
    //    //
    //    // Summary:
    //    //     The semicolon key.
    //    Semicolon = 123,
    //    //
    //    // Summary:
    //    //     The slash key.
    //    Slash = 124,
    //    //
    //    // Summary:
    //    //     The Sleep key.
    //    Sleep = 125,
    //    //
    //    // Summary:
    //    //     The Spacebar.
    //    Space = 126,
    //    //
    //    // Summary:
    //    //     The Stop key.
    //    Stop = 127,
    //    //
    //    // Summary:
    //    //     The Print Screen key.
    //    PrintScreen = 128,
    //    //
    //    // Summary:
    //    //     The Tab key.
    //    Tab = 129,
    //    //
    //    // Summary:
    //    //     The Japanese Underline key.
    //    Underline = 130,
    //    //
    //    // Summary:
    //    //     An unlabeled key.
    //    Unlabeled = 131,
    //    //
    //    // Summary:
    //    //     The Up Arrow key.
    //    UpArrow = 132,
    //    //
    //    // Summary:
    //    //     The Volume Down key.
    //    VolumeDown = 133,
    //    //
    //    // Summary:
    //    //     The Volume Up key.
    //    VolumeUp = 134,
    //    //
    //    // Summary:
    //    //     The Wake key.
    //    Wake = 135,
    //    //
    //    // Summary:
    //    //     The Web Backwards key.
    //    WebBack = 136,
    //    //
    //    // Summary:
    //    //     The Web Favorites key.
    //    WebFavorites = 137,
    //    //
    //    // Summary:
    //    //     The Web Forwards key.
    //    WebForward = 138,
    //    //
    //    // Summary:
    //    //     The Web Home key.
    //    WebHome = 139,
    //    //
    //    // Summary:
    //    //     The Web Refresh key.
    //    WebRefresh = 140,
    //    //
    //    // Summary:
    //    //     The Web Search key.
    //    WebSearch = 141,
    //    //
    //    // Summary:
    //    //     The Web Stop key.
    //    WebStop = 142,
    //    //
    //    // Summary:
    //    //     The Japanese Yen key.
    //    Yen = 143,
    //    UNKNOWN = 144,
    //}
}
