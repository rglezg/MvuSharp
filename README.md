# MvuSharp: Model-View-Update framework for C#

[![Gitter](https://badges.gitter.im/MvuSharp/community.svg)](https://gitter.im/MvuSharp/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

This is my attempt to create a cross-platform framework for developing any type of C# application using the
Model-View-Update pattern aka [the Elm architecture](https://guide.elm-lang.org/architecture/). Although the main features already work, it is still experimental and the API is continually
changing, so **DO NOT USE IT IN PRODUCTION!**

This project was originally based on [Elmish](https://github.com/elmish/elmish), and also manages impure behaviors
through a variant of the Mediator/Service-Locator Pattern you can find on [MediatR](https://github.com/jbogard/MediatR).