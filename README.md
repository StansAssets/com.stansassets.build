# Build System

Build system package is a free open source product that contains Unity Build related API to help you with Unity build configuration & automation as well as providing the ability to get build metadata in runtime. 

[![NPM Package](https://img.shields.io/npm/v/com.stansassets.build)](https://www.npmjs.com/package/com.stansassets.build)
[![openupm](https://img.shields.io/npm/v/com.stansassets.build?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.stansassets.build/)
[![Licence](https://img.shields.io/npm/l/com.stansassets.build)](https://github.com/StansAssets/com.stansassets.build/blob/master/LICENSE)
[![Issues](https://img.shields.io/github/issues/StansAssets/com.stansassets.build)](https://github.com/StansAssets/com.stansassets.build/issues)

#### Quick links to explore the library:
* [Get Build Metadata at runtime.](https://myapi)
* [Automated build number](https://myapi)
* [Build steps configuration](https://myapi)

[API Reference](https://myapi) | [Forum](https://myforum) | [Wiki](https://github.com/StansAssets/com.stansassets.build/wiki)

### Install from NPM
* Navigate to the `Packages` directory of your project.
* Adjust the [project manifest file](https://docs.unity3d.com/Manual/upm-manifestPrj.html) `manifest.json` in a text editor.
* Ensure `https://registry.npmjs.org/` is part of `scopedRegistries`.
  * Ensure `com.stansassets` is part of `scopes`.
  * Add `com.stansassets.facebook` to the `dependencies`, stating the latest version.

A minimal example ends up looking like this. Please note that the version `X.Y.Z` stated here is to be replaced with [the latest released version](https://www.npmjs.com/package/com.stansassets.build) which is currently [![NPM Package](https://img.shields.io/npm/v/com.stansassets.build)](https://www.npmjs.com/package/com.stansassets.build).
  ```json
  {
    "scopedRegistries": [
      {
        "name": "npmjs",
        "url": "https://registry.npmjs.org/",
        "scopes": [
          "com.stansassets"
        ]
      }
    ],
    "dependencies": {
      "com.stansassets.build": "X.Y.Z",
      ...
    }
  }
  ```
* Switch back to the Unity software and wait for it to finish importing the added package.

### Install from OpenUPM
* Install openupm-cli `npm install -g openupm-cli` or `yarn global add openupm-cli`
* Enter your unity project folder `cd <YOUR_UNITY_PROJECT_FOLDER>`
* Install package `openupm add com.stansassets.facebook`
