module Tests

open System
open CarSharp
open Microsoft.FSharp.Core
open global.Xunit
open FsCheck.Xunit




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

let isOk (result: Result<'a, 'b>) = result.IsOk

[<Property>]
let ``Remove cars to Fleet`` (fleet: Fleet) (car: Car) =
    fleet
    |> add car
    |> remove car
    |> isOk

[<Property>]
let ``an available car can be rent`` (fleet: Fleet) (car: Available) =
    fleet
    |> add (AvailableCar car)
    |> rent car
    |> isOk

[<Property>]
let ``an available car not in the fleet cannot be rent`` (fleet: Fleet) (car: Available) =
    let result =
        fleet
        |> rent car
    result.IsError

// [<Property>]
// let ``a rented car can be returned`` (fleet: Fleet) (car: Rent) =
//     let result =
//         fleet
//         |> add (RentCar car)
//         |> book car
//     result.IsOk

let ``a rent car `` (fleet: Fleet) (car: Available) =
    fleet
    |> add (AvailableCar car)
    |> rent car

