namespace EmberMemory.Listener
{
    public struct ProcessLifeTimeReport<T>
    {
        public bool Terminated { get; set; }
        public T Report { get; set; }
    }
}
