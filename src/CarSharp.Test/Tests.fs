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
    
[<Property>]
let ``Add Car to Fleet`` availableCar =
    let addResult = add availableCar emptyFleet

    Assert.Equal(1u , count addResult)
    
[<Property>]
let ``Remove Car from Empty Fleet have Error`` availableCar =
    let removedResult = remove availableCar emptyFleet
    Assert.True(removedResult.IsError)
 
[<Property>]
let ``Add cars to Fleet`` (fleet: Fleet) (car: Car) =
    count (add car fleet) = count fleet + 1u

let allCars n =
    [1 .. int n]
    |> List.map ( fun _ -> AvailableCar)

let foldCars f fleet cars =
    cars
    |> List.fold (fun fleet car ->
    f fleet car |> getOk) fleet

[<Property>]
let ``Remove cars to Fleet`` (fleet: Fleet) (car: Car) =
    let newFleet =
        fleet
        |> add car

    let result = remove car newFleet
    result.IsOk

let getGuid (car: Car) : Guid =
    match car with
    | AvailableCar (Available guid) -> guid
    | RentCar (Rent guid) -> guid

let book (car: Available) (fleet: Fleet) : Result<Fleet, Error> =

    let found: Car option =
        fleet |> find (AvailableCar car)

    match found with
    | None -> Result.Error "No available car to rent"
    | Some foundCar ->
        let without = fleet |> remove foundCar |> getOk
        let guid = foundCar |> getGuid
        without
        |> (add (RentCar (Rent guid)))
        |> Result.Ok

[<Property>]
let ``an available car can be rent`` (fleet: Fleet) (car: Available) =
    let result =
        fleet
        |> add (AvailableCar car)
        |> book car
    result.IsOk

[<Property>]
let ``an available car not in the fleet cannot be rent`` (fleet: Fleet) (car: Available) =
    let result =
        fleet
        |> book car
    result.IsError
//
// [<Property>]
// let ``a not available car can be returned`` (fleet: Fleet) (car: Rent) =
//     let result =
//         fleet
//         |> add (RentCar car)
//         |> book car
//     result.IsOk
