package com.example.domain.usecase

import com.example.domain.services.RideService
import org.koin.core.KoinComponent

class GetBaseRoutesBusesUseCase(private val getRideServiceRepository: RideService) : KoinComponent {

    operator fun invoke(destination: Int) =
        getRideServiceRepository.getLocalServiceRideInformation(destination)
}