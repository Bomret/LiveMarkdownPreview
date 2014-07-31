#r @"FAKE\tools\FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

RestorePackages()

let name ="LiveMarkdownPreview"
let description = "A small tool that watches a selected markdown file for changes and displays them in real time."
let id = "949c5de6-a78e-4479-82de-f385d96a9ca0"
let authors = ["Stefan Reichel"]
let tags = "markdown, preview, html"

let builtAssembly = name + ".exe"
let publishDir = "./publish"
let buildDir = "./build"
let testDir = "./test"

let version =
    match buildServer with
        | TeamCity -> buildVersion
        | _ -> "0.5.0"

Target "Clean" (fun _ -> CleanDirs [buildDir; testDir; publishDir])

Target "BuildApp" (fun _ ->
    CreateCSharpAssemblyInfo ("./" + name + "/Properties/AssemblyInfo.cs")
        [Attribute.Title name
         Attribute.Description description
         Attribute.Guid id
         Attribute.Product name
         Attribute.Version version
         Attribute.FileVersion version]

    MSBuildRelease buildDir "Build" [(name @@ name + ".csproj")]
    |> Log "Building lib: "
)

Target "BuildTests" (fun _ ->
    !! "*.Tests/**/*.csproj"
    |> MSBuildDebug testDir "Build"
    |> Log "Building tests: "
)

Target "Test" (fun _ ->
    !! (testDir @@ "*.Tests.dll")
    |> MSpec (fun p -> {p with HtmlOutputDir = testDir})
)

"Clean"
    ==> "BuildApp"
    ==> "BuildTests"
    ==> "Test"

RunTargetOrDefault "Test"
