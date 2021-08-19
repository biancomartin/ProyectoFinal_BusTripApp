package com.example.data.mapper

import com.example.data.response.*
import com.example.domain.response.*

fun transformListRecorridoBaseResponseToListRecorridoBaseInformation(list: List<RecorridoBaseResponse>): List<RecorridoBaseInformation> =
    list.map {
        it.toInformation()
    }

fun RecorridoBaseResponse.toInformation() =
    RecorridoBaseInformation(
        recorridoId,
        linea,
        transformListCoordinatesResponseToListCoordinates(coordenadas)
    )

fun transformListCoordinatesResponseToListCoordinates(list: List<CoordinateResponse>): List<Coordinates> =
    list.map {
        Coordinates(it.lat, it.lng)
    }

fun transformPositionRecorridoResponseToRecorridoIntermedio(response: PositionRecorridoResponse): RecorridoIntermedio =
    RecorridoIntermedio(
        recorridoId = response.recorridoId,
        coordenadasIntermedias = transformListCoordinatesResponseToListCoordinates(response.coordenadasIntermedias)
    )


fun transformListRecorridosMultipleLinesResponseToListRecorridoBaseInformation(list: List<RecorridosMultipleLinesResponse>): List<TravelLineInformation> =
    list.map {
        it.toMultipleLinesTravelInfo()
    }

fun RecorridosMultipleLinesResponse.toMultipleLinesTravelInfo() =
    TravelLineInformation(
        trayecto,
        linea,
        transformListCoordinatesResponseToListCoordinates(coordenadas)
    )

fun transformListTravelBodyBEResponseToListTravelBodyInformation(list: List<TravelBodyBEResponse>): List<TravelBody> =
    list.map {
        it.toTravelBody()
    }

fun TravelBodyBEResponse.toTravelBody(): TravelBody = TravelBody(
    tiempo,
    distancia,
    linea,
    trayecto,
    coordenadaOrigen.toModel(), coordenadaDestino.toModel()
)

fun CoordinateResponse.toModel(): Coordinates = Coordinates(lat, lng)