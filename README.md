# Pragmatic Clean Code

This repository contains the code examples for *Pragmatic Clean Code* by Krystyna Ślusarczyk.

## Structure

Each chapter has its own project, following the naming pattern:

```
Chapter_01
Chapter_02
Chapter_03
...
```

If a chapter includes a refactoring case study, it also comes with two additional projects: a "before" version and an "after" version.

```
Chapter_01_RefactoringCaseStudy_Before
Chapter_01_RefactoringCaseStudy_After
```

## A note on the code examples

Outside of the refactoring case studies, the clean and messy code examples in this book are simplified to illustrate a specific point, and are not meant to be run as-is - many don't include a full, working implementation. They do include comments to guide you, though the full explanations are in the book itself.

The refactoring case studies are different: the code there is fully working. Each project is a console app, so feel free to write your own code in it, comment things out, and experiment freely.

## Getting started

**Download the code**

You can either clone the repository with Git:

```bash
git clone <repository-url>
```

or download it as a ZIP file using the "Code" button on GitHub and extract it locally.

**Run the examples**

You'll need an IDE that supports .NET 10, such as:

- Visual Studio Community 2026
- Visual Studio Code with the C# Dev Kit extension

Open the solution or the project folder for the chapter you're currently reading, and build and run it from your IDE as you normally would.
