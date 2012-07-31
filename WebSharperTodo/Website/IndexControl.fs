namespace Website

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.JQuery
open IntelliFactory.WebSharper.JQueryUI

open Website

module Todo =
    [<JavaScript>]
    let moveDroppedItem (item:JQuery) (dropZone:JQuery) = 
        item.AppendTo(dropZone).Ignore

    [<JavaScript>]
    let initDrag element = 
        let config = 
            DraggableConfiguration(
                revert = true, cursor = "move", helper = "clone")
        Draggable.New(element, config) |> ignore

    [<JavaScript>]
    let initDrop element = 
        let config = 
            DroppableConfiguration(
                hoverClass = "ui-state-active", accept = ".draggable")
        let dropZone = Droppable.New(element, config) 
        dropZone.OnDrop( fun ev element -> 
            moveDroppedItem <| JQuery.Of(element.Dom) <| JQuery.Of(ev.CurrentTarget) )

    [<JavaScript>]
    let main tasks = 
        Div [ yield Attr.Class "tasks droppable";  
            for task in tasks do
                yield Div [ 
                    Attr.Class "ui-widget-content draggable"; Text task] 
                    |>! initDrag :> IPagelet
        ] |>! initDrop   

type IndexControl() =
    inherit Web.Control()

    [<DefaultValue>]
    val mutable Tasks : string list

    [<JavaScript>]
    override x.Body =
        upcast Todo.main x.Tasks