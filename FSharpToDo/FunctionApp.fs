module FSharpToDo.FunctionApp
open System
open FSharpToDo
open FunctionMonkey.FSharp.Configuration
open FunctionMonkey.FSharp.Models
open AccidentalFish.FSharp.Validation

type AddToDoItemCommand = {
    title: string
}

type MarkAsCompleteCommand = {
    id: Guid
}

let validateMarkAsCompleteCommand = createValidatorFor<MarkAsCompleteCommand>() {
    validate (fun cmd -> cmd.id) [
        isNotEqualTo Guid.Empty
    ]
}

let validateAddToDoItemCommand = createValidatorFor<AddToDoItemCommand>() {
    validate (fun cmd -> cmd.title) [
        isNotNull
        isNotEmpty
        hasMinLengthOf 1
    ]
}

let addToDoItem addToRepository command =
    addToRepository command.title
    
let markAsComplete markAsCompleteInRepository command =
    markAsCompleteInRepository command.id
    
let isResultValid result = match result with | Ok -> true | _ -> false

let app = functionApp {
    isValid isResultValid
    httpRoute "todo" [
        azureFunction.http (Handler(addToDoItem MemoryRepository.add), Post, validator = validateAddToDoItemCommand)
        azureFunction.http (Handler(MemoryRepository.getAll), Get)
        azureFunction.http (Handler(markAsComplete MemoryRepository.markComplete), Put, validator = validateMarkAsCompleteCommand)
    ]
}
