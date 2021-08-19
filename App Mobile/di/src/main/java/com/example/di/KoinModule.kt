package com.example.di

import com.example.data.NetworkingConfigHelper
import com.example.data.impl.CalculadorTiemposServiceImpl
import com.example.data.impl.RideServiceImpl
import com.example.domain.services.AlgorithmsService
import com.example.domain.services.RideService
import com.example.domain.usecase.*
import org.koin.dsl.module

val useCasesModule = module {
    single { GetBaseRoutesBusesUseCase(get()) }
    factory { GetLinesBusesUseCase(get()) }
    single { ExecuteTypeAlgorithmUseCase(get()) }
    single { GetRecorridoEntrePuntosSeleccionados(get()) }
    single { GetListMultipleLinesTravelInfoUseCase(get()) }

}

val repositoryModule = module {
    single<RideService> { RideServiceImpl() }
    single<AlgorithmsService> { CalculadorTiemposServiceImpl() }
}

val networkingModule = module {
    single {
        NetworkingConfigHelper.createRetrofitInstance()
    }
}