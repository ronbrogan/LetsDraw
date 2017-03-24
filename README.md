# LetsDraw
This is a pet project with the goal to make a general purpose 3D rendering engine for video game development.

## Details
The codebase is a Visual Studio 2015 solution which has only been tested to function on Windows 10 with Nvidia graphics cards. Your mileage may vary.

### Quirk with SceneComposer
The app uses the OpenTK GLControl to host the OpenGL context in WPF, however the current NuGet version of GLControl is incompatible with the current version of OpenTK. To work around this issue, I have built the GLControl from source and have checked it in.