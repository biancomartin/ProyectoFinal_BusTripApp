package com.example.data.impl

import com.example.data.mapper.BusLineMapper
import com.example.data.mapper.transformListRecorridoBaseResponseToListRecorridoBaseInformation
import com.example.data.mapper.transformListRecorridosMultipleLinesResponseToListRecorridoBaseInformation
import com.example.data.service.ServiceApi
import com.example.data.service.ServiceGenerator
import com.example.domain.response.*
import com.example.domain.services.RideService
import org.koin.core.KoinComponent

class RideServiceImpl : RideService, KoinComponent {

    private val api = ServiceGenerator()
    override fun getLocalServiceRideInformation(destination: Int): UseCaseResult<List<RecorridoBaseInformation>> {
        val call =
            api.createService(ServiceApi::class.java)
                .getServiceBaseRouteInformation(destination.toString())

        try {
            val response = call.execute()
            if (response.isSuccessful) {
                val body = response.body()
                if (body != null) {
                    return UseCaseResult.Success(
                        transformListRecorridoBaseResponseToListRecorridoBaseInformation(body)
                    )
                } else {
                    return UseCaseResult.Failure(Exception("failed"))
                }
            }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }

        return UseCaseResult.Failure(Exception("response not success"))
    }

    override fun getLinesInformation(): UseCaseResult<List<LineBus>> {

        val call = api.createService(ServiceApi::class.java).getListOfBuses()

        val mapper = BusLineMapper()
        try {
            val response = call.execute()
            if (response.isSuccessful)
                response.body()?.let {
                    mapper.transformListOfBuses(it)
                }?.let {
                    return UseCaseResult.Success(it)
                }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }
        return UseCaseResult.Failure(Exception(""))
    }


    override fun getRecorridoEntrePuntosSeleccionados(puntosSeleccionados: PositionMultipleLines): UseCaseResult<List<TravelLineInformation>> {

        val call =
            api.createService(ServiceApi::class.java)
                .getParadasCercanasMultiplesLineas(puntosSeleccionados)

        try {
            val response = call.execute()
            if (response.isSuccessful)
                response.body()?.let {
                    transformListRecorridosMultipleLinesResponseToListRecorridoBaseInformation(it)
                }?.let {
                    return UseCaseResult.Success(it)
                }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }
        return UseCaseResult.Failure(Exception(""))
    }

    override fun getMultipleLinesSearching(destination: PositionMultipleLines): UseCaseResult<List<TravelLineInformation>> {
        val call =
            api.createService(ServiceApi::class.java)
                .getParadasCercanasMultiplesLineas(destination)

        try {
            val response = call.execute()
            if (response.isSuccessful) {
                val body = response.body()
                if (body != null) {
                    return UseCaseResult.Success(
                        transformListRecorridosMultipleLinesResponseToListRecorridoBaseInformation(
                            body
                        )
                    )
                } else {
                    return UseCaseResult.Failure(Exception("failed"))
                }
            }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }

        return UseCaseResult.Failure(Exception("response not success"))
    }
}