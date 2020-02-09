module FSharpToDo.MemoryRepository
open FSharpToDo
open ToDoItem

let mutable private items : ToDoItem[] = [||]

let add title =
    let newItem = {
        title = title
        id = System.Guid.NewGuid()
        isComplete = false
    }
    items <- Array.append items [|newItem|]
    newItem
    
let getAll () = items

let markComplete id =
    items <-
        items
        |> Array.map (fun i -> match i.id = id with
                               | true -> { i with isComplete = true }
                               | false -> i)
    
     