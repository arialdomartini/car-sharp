module CarSharp

open System

type Seats = One | Next of Seats
    with
    member this.Value : uint =
        let rec helper v =
            match v with
            | One -> 1u
            | Next x -> 1u + helper x
        helper this
    static member Of (value : uint) =
        let rec helper i =
            match i with
            | 0u -> failwith "fatal error"
            | 1u -> One
            | v -> Next (helper (v - 1u))
        helper value


let one = One
let two = Next One
let three = Next (Next One)

type CarDetails =
    { id: Guid
      seats: Seats }

type Available = Available of CarDetails
type Rent = Rent of CarDetails

type Car =
   | AvailableCar of Available
   | RentCar of Rent

type BulkOrder = BulkOrder of Available list

type Booking = Booking of Rent list

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

let availableCarList (Fleet cars): Available list =
    let isAvailable (car: Car): Option<Available> =
        match car with
        | AvailableCar available -> Some available
        | _ -> None

    cars
    |> List.choose isAvailable

let findAvailable (available: Available) (fleet: Fleet) : Available option =
    availableCarList fleet
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

let getDetails (car: Car) : CarDetails =
    match car with
    | AvailableCar (Available details) -> details
    | RentCar (Rent details) -> details

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

let rec powerSet list =
    seq {
        match list with
        | [] -> yield []
        | x :: xs ->
            // Per ogni sottoinsieme del resto della lista
            for set in powerSet xs do
                // Genera uno senza x
                yield set
                // E uno con x
                yield x :: set
    }


let flip fn b a = fn a b
let addRange cars fleet = cars |> List.fold (flip add) fleet
let rentRange (BulkOrder availableCars) fleet =
    availableCars
    |> List.fold
        (fun fleetStepResult available ->
            fleetStepResult
            |> Result.bind (rent available))
        (Result.Ok fleet)

let rentWithSeats (seats: Seats) (fleet: Fleet) : Result<Fleet * Booking, Error> =
    let listAllPossible (seats: Seats) (Fleet carList): (Fleet * Booking) =
        let seatTotal (candidate: Available list): Seats =
            candidate
            |> List.sumBy (fun (Available details) -> details.seats.Value)
            |> Seats.Of

        let found =
            seq {
                for candidate in availableCarList fleet |> powerSet do
                    if (seatTotal candidate).Value >= (seats.Value) then
                        let result = rentRange (BulkOrder candidate) fleet
                        match result with
                        | Ok okRes -> yield! [candidate, okRes]
                        | Error _ -> yield! []
            }

        let (candidate, remainingFleet) =
            found
            |> Seq.minBy (fun (r: Available list, _) -> (seatTotal r).Value)

        (remainingFleet,
         candidate
         |> List.map (fun (Available details) -> Rent details)
         |> Booking)