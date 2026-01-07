module Tests

open System
open CarSharp
open Microsoft.FSharp.Core
open global.Xunit
open FsCheck.Xunit


let getOk( result: Result<'a, 'b>) =
    match result with
    | Ok resultValue -> resultValue
    | Error errorValue -> failwith "Should be OK"

[<Fact>]
let ``Empty Fleet has no vehicles`` () =
    Assert.Equal(0u, count emptyFleet)
    
[<Fact>]
let ``Add Car to Fleet`` () = 
    let addResult = add emptyFleet Car
    Assert.Equal(1u , count (getOk addResult))
    
[<Fact>]
let ``Remove Car from Empty Fleet have Error`` () = 
    let RemovedResult = remove emptyFleet Car
    Assert.True(RemovedResult.IsError)
 
[<Property>]
let ``Add cars to Fleet`` (toAddCars: uint) =
    let cars = 
        [1 .. int toAddCars]
        |> List.map ( fun _ -> Car)
        |> List.fold (fun fleet car ->
            add fleet car |> getOk) emptyFleet
    count cars = toAddCars

let allCars n =
    [1 .. int n]
    |> List.map ( fun _ -> Car)

let foldCars f fleet cars =
    cars
    |> List.fold (fun fleet car ->
    f fleet car |> getOk) fleet

    
[<Property>]
let ``Remove cars to Fleet`` (a: uint, b: uint) =
    let cars =
        allCars (a + b)
        |> foldCars add emptyFleet
    let removedCars =
        allCars b
        |> foldCars remove cars
    count removedCars = a
