﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ThreePM.MusicPlayer;
using ThreePM.MusicLibrary;
using System.IO;

namespace ThreePM.Engine
{
    public static class Main
    {
        #region Declarations

        private static string tempPlayList;
        private static MusicPlayer.Player m_player;
        private static MusicLibrary.Library m_library;
        private static ThreePM.Utilities.HttpServer m_server;

        #endregion

        #region Properties

        public static Player Player
        {
            get
            {
                return m_player;
            }
            set
            {
                m_player = value;
            }
        }

        public static Library Library
        {
            get
            {
                return m_library;
            }
            set
            {
                m_library = value;
            }
        }

        public static bool HttpServerEnabled
        {
            get
            {
                return m_server != null;
            }
            set
            {
                if (value)
                {
                    if (m_server == null)
                    {
                        m_server = new ThreePM.Utilities.HttpServer(Player, Library);
                    }
                }
                else
                {
                    if (m_server != null)
                    {
                        m_server.Dispose();
                        m_server = null;
                    }
                }
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Gets stuff ready so you can hook up event handlers
        /// </summary>
        public static void Initialize()
        {
            Initialize(-1);
        }

        public static void Initialize(int deviceNumber)
        {
            Player = new MusicPlayer.Player(deviceNumber);
            Player.SongInfoFormatString = Utilities.GetValue("Player.SongInfoFormatString", "{Artist} - {Title}");
            Player.SecondsBeforeUpdatePlayCount = Utilities.GetValue("Player.SecondsBeforeUpdatePlayCount", 20);
            Player.IgnorePreviouslyPlayedSongsInRandomMode = Utilities.GetValue("Player.IgnorePreviouslyPlayedSongsInRandomMode", false);
            Player.NeverPlayIgnoredSongs = Utilities.GetValue("Player.NeverPlayIgnoredSongs", false);
            Player.AudioscrobblerEnabled = Utilities.GetValue("Player.AudioscrobblerEnabled", false);
            Player.AudioscrobblerUserName = Utilities.GetValue("Player.AudioscrobblerUserName", "");
            Player.AudioscrobblerPassword = Utilities.GetValue("Player.AudioscrobblerPassword", "");


            Player.RepeatCurrentTrack = Convert.ToBoolean(Utilities.GetValue("Player.RepeatCurrentTrack", false));
            Player.Playlist.PlaylistStyle = (PlaylistStyle)Utilities.GetValue("Player.PlaylistStyle", 0);

            Library = new MusicLibrary.Library();
        }

        /// <summary>
        /// Does the stuff
        /// </summary>
        public static void Start()
        {
            tempPlayList = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ThreePM.m3u";


            Player.Library = Library;


            // load the last song that was being played
            string file = Utilities.GetValue("Player.CurrentSong", "");
            if (!String.IsNullOrEmpty(file))
            {
                // don't count the play since we're just restarting the same song
                float position = Utilities.GetValue("Player.Position", 0f);
                if (Player.LoadFile(file, Convert.ToInt32(position) <= Player.SecondsBeforeUpdatePlayCount))
                {
                    Player.Position = position;
                    Player.Play();
                }
            }

            Player.Playlist.LoadFromFile(tempPlayList);

            HttpServerEnabled = Convert.ToBoolean(Utilities.GetValue("MainForm.HttpServer", false));
        }

        /// <summary>
        /// Finishes the stuff
        /// </summary>
        public static void End()
        {
            Player.Pause();
            File.Delete(tempPlayList);
            Player.Playlist.SaveToFile(tempPlayList);

            if (Player.CurrentSong != null)
            {
                Utilities.SetValue("Player.CurrentSong", (object)Player.CurrentSong.FileName);
                Utilities.SetValue("Player.Position", (object)Player.Position);
            }

            if (m_server != null)
            {
                m_server.Dispose();
                m_server = null;
            }
            Player.Dispose();
            Library.Dispose();
            Player = null;
            Library = null;
        }

        #endregion
    }
}
