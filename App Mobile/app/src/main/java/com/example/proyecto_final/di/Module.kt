package com.example.proyecto_final.di

import com.example.proyecto_final.ui.fragments.FragmentTravelPredictionViewModel
import com.example.proyecto_final.ui.fragments.MapFragmentViewModel
import com.example.proyecto_final.ui.fragments.SettingsViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

@JvmField
val viewModelModule = module {
    viewModel { MapFragmentViewModel() }
    viewModel { SettingsViewModel() }
    viewModel { FragmentTravelPredictionViewModel(get()) }
}