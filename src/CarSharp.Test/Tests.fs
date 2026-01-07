module Tests

open System
open Microsoft.FSharp.Core
open global.Xunit
open FsCheck.Xunit

type Car = Car
type Fleet = Fleet of Car list
type FleetCount = uint
type Error = String

let emptyFleet = Fleet []

let count (Fleet cars): FleetCount =
    uint cars.Length

let add (Fleet cars) (car: Car): Result<Fleet, Error> =
    let newFleet = car::cars
    Ok(Fleet newFleet)
    
let remove (Fleet cars) (car: Car): Result<Fleet, Error> =
    match cars with
    | [] -> Result.Error "Empty Fleet"
    | (_::tail) -> Result.Ok (Fleet tail)
    
// let count (fleet: Fleet): FleetCount =
//     match fleet with
//     | Fleet cars -> uint cars.Length

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
        [1 .. (int) toAddCars]
        |> List.map ( fun _ -> Car)
        |> List.fold (fun fleet car ->
            add fleet car |> getOk) emptyFleet
    count cars = toAddCars