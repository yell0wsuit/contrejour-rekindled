using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using Mokus2D;
using Mokus2D.Util.MathUtils;

namespace Default.Namespace
{
    public class SoundManager : IUpdatable
    {
        public SoundManager()
        {
            Loop = true;
            MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(OnMediaStateChanged);
        }

        private void OnMediaStateChanged(object sender, EventArgs eventArgs)
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                if (!pausing)
                {
                    MusicDisableEvent.SendEvent();
                    refreshSong = true;
                }
                pausing = false;
            }
        }

        public bool SoundEnabled
        {
            get => soundEnabled;
            set
            {
                soundEnabled = value;
                SoundEffect.MasterVolume = value ? 1 : 0;
            }
        }

        public bool MusicEnabled
        {
            get => musicEnabled;
            set
            {
                musicEnabled = value;
                if (HasControl)
                {
                    if (value && currentMusic != null)
                    {
                        if (refreshSong)
                        {
                            GetControlAndPlay();
                        }
                        else
                        {
                            MediaPlayer.Resume();
                        }
                        refreshSong = false;
                    }
                    else
                    {
                        Pause();
                    }
                    RefreshLoop();
                    return;
                }
                if (value)
                {
                    GetControlAndPlay();
                }
            }
        }

        private void GetControlAndPlay()
        {
            hasControl = true;
            DoPlayCurrentSong();
        }

        private void RefreshLoop()
        {
            if (HasControl && MediaPlayer.IsRepeating != Loop)
            {
                MediaPlayer.IsRepeating = Loop;
            }
        }

        public void OnGameActivated()
        {
            hasControl = false;
        }

        public bool Loop
        {
            get => loop;
            set
            {
                if (HasControl)
                {
                    MediaPlayer.IsRepeating = value;
                }
                loop = value;
            }
        }

        public bool HasControl => MediaPlayer.GameHasControl || hasControl;

        public void Update(float time)
        {
        }

        public void PreloadSongs(string[] paths)
        {
            PreloadItems(paths, songs);
        }

        public void PreloadSounds(string[] paths)
        {
            PreloadItems(paths, sounds);
        }

        private void PreloadItems<T>(string[] paths, Dictionary<string, T> cache)
        {
            foreach (string text in paths)
            {
                PreloadItem(text, cache);
            }
        }

        public void PreloadSong(string path)
        {
            PreloadItem(path, songs);
        }

        private void PreloadItem<T>(string path, Dictionary<string, T> cache)
        {
            if (!cache.ContainsKey(path))
            {
                T t = Mokus2DGame.ContentManager.Load<T>(Path.Combine(MusicPath, path));
                cache[path] = t;
            }
        }

        public void PreloadSound(string path)
        {
            PreloadItem(path, sounds);
        }

        public void StopMusic()
        {
            if (HasControl)
            {
                MediaPlayer.Stop();
            }
            StopScheduledMusic();
            currentMusic = null;
        }

        public void RemoveCurrentMusic()
        {
            currentMusic = null;
        }

        public void PlayRandomSound(string[] files, float volume = 1f)
        {
            PlaySound(files.RandomItem(), volume, 0f, 0f);
        }

        public void PlayRandomSound(List<string> files, float volume = 1f)
        {
            int num = Maths.Random(files.Count);
            PlaySound(files[num], volume, 0f, 0f);
        }

        public void PlaySound(string path, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            PreloadSound(path);
            SoundEffect soundEffect = sounds[path];
            _ = soundEffect.Play(volume, pitch, pan);
        }

        public void PlayMusic(Song song)
        {
            if (currentMusic == null)
            {
                currentMusic = song;
                DoPlayCurrentSong();
                return;
            }
            if (currentMusic != null && song != currentMusic)
            {
                currentMusic = song;
                StopScheduledMusic();
                if (HasControl)
                {
                    MediaPlayer.Stop();
                }
                changeMusicAction = Mokus2DGame.Scheduler.Schedule(new Action(DoPlayCurrentSong), SongChangePause);
            }
        }

        private void StopScheduledMusic()
        {
            if (changeMusicAction != null)
            {
                changeMusicAction.Dispose();
                changeMusicAction = null;
            }
        }

        public void PlayMusic(string path)
        {
            PreloadSong(path);
            PlayMusic(songs[path]);
        }

        private void DoPlayCurrentSong()
        {
            changeMusicAction = null;
            if (HasControl)
            {
                MediaPlayer.Play(currentMusic);
                RefreshLoop();
                if (!MusicEnabled)
                {
                    Pause();
                }
            }
        }

        private void Pause()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                pausing = true;
                MediaPlayer.Pause();
            }
        }

        private readonly Dictionary<string, Song> songs = new();

        private readonly Dictionary<string, SoundEffect> sounds = new();

        public string MusicPath = "";

        public float SongChangePause = 1f;

        public readonly EventSender MusicDisableEvent = new();

        private IDisposable changeMusicAction;

        private Song currentMusic;

        protected bool musicEnabled = true;

        protected bool paused;

        private bool soundEnabled = true;

        private bool loop = true;

        private bool hasControl;

        private bool pausing;

        private bool refreshSong;
    }
}
