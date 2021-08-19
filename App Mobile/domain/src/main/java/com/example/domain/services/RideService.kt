package com.example.domain.services

import com.example.domain.response.*

interface RideService {
    fun getLocalServiceRideInformation(destination: Int): UseCaseResult<List<RecorridoBaseInformation>>
    fun getLinesInformation(): UseCaseResult<List<LineBus>>
    fun getRecorridoEntrePuntosSeleccionados(puntosSeleccionados: PositionMultipleLines): UseCaseResult<List<TravelLineInformation>>
    fun getMultipleLinesSearching(puntosSeleccionados: PositionMultipleLines): UseCaseResult<List<TravelLineInformation>>
}