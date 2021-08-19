package com.example.data.impl

import com.example.data.mapper.transformListTravelBodyBEResponseToListTravelBodyInformation
import com.example.data.service.ServiceApi
import com.example.data.service.ServiceGenerator
import com.example.domain.response.TravelBody
import com.example.domain.response.UseCaseResult
import com.example.domain.services.AlgorithmsService
import com.example.domain.usecase.InfoPuntoParadaDomain
import org.koin.core.KoinComponent

class CalculadorTiemposServiceImpl : AlgorithmsService, KoinComponent {

    private val api = ServiceGenerator()

    override fun getCalcularTiempoPorRegresionLinealMultiple(destination: List<*>?): UseCaseResult<List<TravelBody>> {
        val list = destination as List<*>
        val call =
            api.createService(ServiceApi::class.java).calcularTiempoPorRegresionLinealMultiple(
                list as List<InfoPuntoParadaDomain>
            )

        try {

            val response = call.execute()
            if (response.isSuccessful)
                response.body()?.let {
                    transformListTravelBodyBEResponseToListTravelBodyInformation(it)
                }?.let {
                    return UseCaseResult.Success(it)
                }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }

        return UseCaseResult.Failure(Exception("response not success"))
    }

    override fun getCalcularTiempoPorRegresionEnfoqueMatricial(destination: List<*>?): UseCaseResult<List<TravelBody>> {
        val list = destination as List<InfoPuntoParadaDomain>

        val call =
            api.createService(ServiceApi::class.java).calcularTiempoPorRegresionEnfoqueMatricial(
                list
            )

        try {

            val response = call.execute()
            if (response.isSuccessful)
                response.body()?.let {
                    transformListTravelBodyBEResponseToListTravelBodyInformation(it)
                }?.let {
                    return UseCaseResult.Success(it)
                }
        } catch (e: Exception) {
            return UseCaseResult.Failure(e)
        }

        return UseCaseResult.Failure(Exception("response not success"))
    }
}