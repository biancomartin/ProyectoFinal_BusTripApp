package com.example.data.service

import com.example.data.response.ListLineBusResponse
import com.example.data.response.RecorridoBaseResponse
import com.example.data.response.RecorridosMultipleLinesResponse
import com.example.data.response.TravelBodyBEResponse
import com.example.domain.response.PositionMultipleLines
import com.example.domain.usecase.InfoPuntoParadaDomain
import retrofit2.Call
import retrofit2.http.*

interface ServiceApi {
    @GET("/RecorridoBase/ObtenerRecorridosPorLinea")
    fun getServiceBaseRouteInformation(
        @Query("linea") linea: String
    ): Call<List<RecorridoBaseResponse>>

    @GET("/RecorridoBase/ObtenerRecorridos")
    fun getListOfBuses(): Call<List<ListLineBusResponse>>

    ////////// Calculo de tiempos y distancia

    @Headers("Content-Type: application/json;charset=UTF-8")
    @POST("/CalculadorTiempo/ObtenerTiempoPorRegresionAcumulado")
    fun calcularTiempoPorRegresionLinealMultiple(
        @Body user: List<InfoPuntoParadaDomain>
    ): Call<List<TravelBodyBEResponse>>

    @POST("/CalculadorTiempo/ObtenerTiempoPorRegresionDiferenciaDeCeldas")
    fun calcularTiempoPorRegresionEnfoqueMatricial(
        @Body user: List<InfoPuntoParadaDomain>
    ): Call<List<TravelBodyBEResponse>>

    //////////

    @POST("/RecorridoBase/ParadasCercanas")
    fun getParadasCercanasMultiplesLineas(
        @Body puntosSeleccionados: PositionMultipleLines
    ): Call<List<RecorridosMultipleLinesResponse>>
}