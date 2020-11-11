<!-- @bot-written -->
<!-- % protected region % [Configure readme here] off begin -->
# lm2348

You have complete ownership of the source code the bots write, which means it is customisable and ready to use for any commercial or non-commercial purposes. Below is an overview of the code to help you get started.
This readme is intended to help get you get started, and can be edited to suit your needs at any time.

**Bot version:** 1.4.6.0 ([Release notes are here](https://parent.codebots.app/library-article/view/569))

## Running your app

We use Docker to enable you to easily run your app locally. To get it running, make sure you have Docker installed on your computer and have the code pulled down from Git. To start the app, use your OS start command script in the [_shortcuts]() folder. Once running, it will be available at [localhost:8000](localhost:8000).

For more detailed instructions on setting up and running with Docker, go here: [link](https://codebots.app/library-article/codebots/view/285)
For instructions on getting it running *without* Docker, go here: [link](https://codebots.app/library-article/codebots/view/49). This is the recommended method of running your application when writing new code.

## Customising bot written code

The bots work like another member of your team, committing their work to the repo so that you can pull it down. It is important that if you want to keep building with the bots, **you commit and push your code before you build**. Pull at any time to get the bot's changes.

### Protected Regions

Wondering how the bots know how to leave your code alone? They use a thing called protected regions. They are areas where you can write code that the bots won't touch. They are scattered just about everywhere throughout the bot-written files, and look something like this:

```
// % protected region % [Add any additional imports here] off begin

// % protected region % [Add any additional imports here] end
```

To use a protected region, change the first comment to say `on` instead of `off`, and the bots will stay away. If you don't turn it on, the bots will keep updating the code inside anytime you build.

For more info on protected regions, read this article: [link](https://codebots.app/library-article/codebots/view/76)

#### Missing a protected region?

Got some code you want to write but no protected region to hold it? You can request new protected regions from our support desk. Go here for instructions on how to do this: [link](https://codebots.app/library-article/codebots/view/515)

### Behaviours

Your app is using: [Developer API](https://codebots.app/library-article/codebots/view/148), [CRUD](https://codebots.app/library-article/codebots/view/7)
You aren't using: [Forms](https://codebots.app/library-article/codebots/view/9), [Timeline](https://codebots.app/library-article/codebots/view/147), [Workflow](https://codebots.app/library-article/codebots/view/8), [User](https://codebots.app/library-article/codebots/view/71)

Behaviours are how you can add in pre-built functionality into your app. There are a whole range of behaviours you can use. Add them into either the Entity or User Interface diagram and get the bots to build to see how they work.

To find out more about the behaviours your bot offers or how to work with a behaviour you have, have a search for the behaviour's name on the learning center, or click on one of the links above.

### Components

Components are visual elements which you can reuse throughout your app to make the building process easier. You can use small components like buttons, or large ones like modals, to save yourself some time. A large majority of the components were also designed with accessibility in mind, so if used properly you can help ensure that your app is as accessible as possible.

For a full list of components and elements available, take a look here: [link](https://codebots.app/library-article/codebots/view/209)

## Server-side

The Server-side is the back end framework which handles data processing and various tasks handled by the server to manage users, data submitted from the client-side and be configured to integrate with external APIs, and ensure application security.

C#Bot uses `ASP.Net Core`, which is an open source and free web framework. For more information on the architecture of C#Bot, you can read this [article](https://codebots.app/library-article/codebots/view/238)

There are several maintenance task a developer will need to use when building their applications. One such tasks is maintaining your local database, this is managed through the [dotnet entity framework CLI tool](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet). You can run this tool in the root of your web server project (`/serverside/src`) using your command line.

If you need to drop your local database at any point you can run the following command.

```
dotnet ef database drop
```

If you are setting up your project for the first time, or have created made some model changes you can use the following two commands to first create a migration for your local database and then apply the migration updates.

```
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

If you would like to know more about setting up and managing your application, take a look [here](https://codebots.app/library-article/codebots/view/190)

## Client-side

Client-side is the interface through visitors of your application connect through with their browsers, It is responsible for displaying information to end users and for sending any information they submit to the server for processing.

C#Bot uses `React` which is an open source JavaScript library for building user interfaces or UI Components.

A simple maintenance task required by developers to perform when first installing the site or when adding a new client-side library is to install the required dependencies or libraries, this can be achieved by running the following command from a terminal in the `clientside` directory

```
yarn install
```

## Running tests

The Test target is required to verify the application is running as intended. It consists of a combination of Unit, Integration and end to end tests which cover functionality in both the client and server-side. For more information on the testing technologies used by C#Bot, take a look [here](https://codebots.app/library-article/codebots/view/424)

To run the client-side tests, navigate to the `clientside` folder and run the following command in your terminal.

```
yarn run test
```

To run the Server-side Unit, Integration or end to end from the terminal you can navigate to each of the dotnet projects inside of your `testtarget` directory and run the following command:

```
dotnet test
```

## Support

If you ever get stuck or want to suggest a change, our support team is here to help! To submit a support request, follow the instructions in this article: [link](https://codebots.app/library-article/codebots/view/77)
<!-- % protected region % [Configure readme here] end -->