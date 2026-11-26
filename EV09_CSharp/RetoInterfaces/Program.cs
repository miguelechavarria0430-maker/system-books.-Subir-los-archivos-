using RetoInterfaces;

Console.WriteLine("=== RETO INTERFACES ===");

MusicPlayer reproductor = new MusicPlayer();

Console.WriteLine("Pista actual: " + reproductor.GetCurrentTrack());

reproductor.LoadTrack("Mi canción favorita");
reproductor.Play();
reproductor.Pause();
reproductor.Stop();

Console.WriteLine("Pista actual: " + reproductor.GetCurrentTrack());
