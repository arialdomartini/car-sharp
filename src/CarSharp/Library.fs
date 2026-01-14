module CarSharp

open System

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

let findAvailable (available: Available) (Fleet cars: Fleet) : Available option =
    let isAvailable (car: Car): Option<Available> =
        match car with
        | AvailableCar available -> Some available
        | _ -> None

    cars
    |> List.choose isAvailable
    |> List.tryFind ((=) available)


let remove (car: Car) (Fleet cars): Result<Fleet, Error> =
    match Fleet cars |> findIndex car with
    | None -> Result.Error "Empty Fleet"
    | Some ix ->
        Result.Ok (Fleet (List.removeAt ix cars))

let getOk (result: Result<'a, 'b>) =
    match result with
    | Ok resultValue -> resultValue
    | Error _ -> failwith "Should be OK"

let getGuid (car: Car) : Guid =
    match car with
    | AvailableCar (Available guid) -> guid
    | RentCar (Rent guid) -> guid

let makeRented (Available guid) =
    Rent guid |> RentCar

type Option<'x> with
   static member toResult (orElse: 'b) (self: 'a option): Result<'a, 'b> =
        match self with
            | Some e -> Result.Ok e
            | None -> Result.Error orElse


type Result<'x, 'y> with
   static member toOption (self: Result<'a, 'b>): Option<'a> =
        match self with
            | Ok e -> Some e
            | Error _ -> None


let bindResult (m: Result<'a, 'b>) (f: 'a -> Result<'a, 'b>) =
    match m with
    | Ok v -> f v
    | Error e -> Result.Error e

type ResultBuilder() =
    member this.Bind(m, f) = bindResult m f
    member this.Return(v) = Result.Ok v

let result = ResultBuilder()

let bindOption (m: Option<'a>) (f: 'a -> Option<'b>) =
    match m with
    | Some v -> f v
    | None -> None

type OptionBuilder() =
    member this.Bind(m, f) = bindOption m f
    member this.Return(v) = Some v

let option = OptionBuilder()

let rent (car: Available) (fleet: Fleet) : Result<Fleet, Error> =
    fleet
    |> findAvailable car
    |> Option.map (fun foundCar ->
        fleet
        |> remove (AvailableCar foundCar)
        |> getOk
        |> add (foundCar |> makeRented))
    |> Option.toResult "No available car to rent"

let tryRemove foundCar fleet =
    fleet
        |> remove (AvailableCar foundCar)
        |> Result.toOption


let rent' (car: Available) (fleet: Fleet) : Result<Fleet, Error> =
    option {
        let! foundCar = fleet |> findAvailable car
        let! removed = fleet |> tryRemove car
        let rentedCar = foundCar |> makeRented
        let result = removed |> add rentedCar
        return result
    }
    |> Option.toResult "No available car to rent"
