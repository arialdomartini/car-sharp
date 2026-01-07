module CarSharp

open System
open System.Linq
open System.Xml.Linq

type Available = Available of Guid
type Rent = Rent of Guid

type Car =
   | AvailableCar of Available
   | RentCar of Rent

type Fleet = Fleet of Car list
type FleetCount = uint
type Error = String

let emptyFleet = Fleet []

let count (Fleet cars): FleetCount =
    uint cars.Length

let add (Fleet cars) (status: Available): Result<Fleet, Error> =
    let newFleet = (AvailableCar status)::cars
    Ok(Fleet newFleet)
    
let remove (Fleet cars) (car: Car): Result<Fleet, Error> =
    match cars with
    | [] -> Result.Error "Empty Fleet"
    | cars ->
        let newFleet = cars.Where((<>) car) |> Seq.toList
        match newFleet with
        | x when x = cars -> Result.Error "No Car removed"
        | _ -> Result.Ok (Fleet newFleet)


// let count (fleet: Fleet): FleetCount =
//     match fleet with
//     | Fleet cars -> uint cars.Length
