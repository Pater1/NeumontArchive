namespace Assets {
    public interface Command<T> {
        void Execute(T on);
    }
}