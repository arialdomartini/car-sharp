module CarSharp

open System

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
