namespace Core.Rendering
{
    public interface IHudElement
    {
        void Draw();

        void Update();
        void Update(double deltaTime);

        void Resize(int width, int height);

        void SetShader(int ProgramHandle);
    }
}
