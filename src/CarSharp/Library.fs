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

let add (car: Car) (Fleet cars): Fleet =
    let newFleet = car::cars
    Fleet newFleet

let findIndex (car: Car) (Fleet cars: Fleet) : int option =
    List.tryFindIndex ((=) car) cars

let find (car: Car) (Fleet cars: Fleet) : Car option =
    List.tryFind ((=) car) cars

let remove (car: Car) (Fleet cars): Result<Fleet, Error> =
    match Fleet cars |> findIndex car with
    | None -> Result.Error "Empty Fleet"
    | Some ix ->
        Result.Ok (Fleet (List.removeAt ix cars))
