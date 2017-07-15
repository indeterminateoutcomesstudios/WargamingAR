﻿using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using WAR.Board;
using WAR.UI;

namespace WAR.Pathfinder.Tests  {
	public class GridTest {
		
		//               ___     ___     ___
		//           ___/8  \___/17 \___/26 \
		//          /7  \___/16 \___/25 \___/ 
		//          \___/6  \___/15 \___/24 \ 
		//          /5  \___/14 \___/23 \___/
		//          \___/4  \___/13 \___/22 \ 
		//          /3	\___/12 \___/21 \___/
		//          \___/2  \___/11 \___/20 \
		//          /1  \___/10 \___/19 \___/
		//          \___/0  \___/9  \___/18 \
		//              \___/   \___/   \___/
		//
		
		[Test]
		public void CostMap() {
			var go = new GameObject();
			// TODO make a mock grid class to test with
			// hex grid instantiates our WARGrid abstact logic
			var grid = go.AddComponent<WARHexGrid>();
			float globalScale = 0.01f;
			
			float outterRadius = 1f * globalScale;
			float innerRadius = outterRadius * Mathf.Sqrt(3) / 2f;
			
			float numberOfColumns = 3f;
			float numberOfRows = 9f;
			
			// create a plane on the origin with an extent of 0.25f
			var plane = new UIPlane {
				center = Vector3.zero,
				extent = new Vector3(3f * numberOfColumns * outterRadius, 0f, numberOfRows * innerRadius)
			};
			var hex = new GameObject("hex cell");
			hex.AddComponent<WARActorCell>();
			hex.AddComponent<MeshRenderer>();
			var child = new GameObject();
			child.transform.SetParent(hex.transform);
			child.AddComponent<TextMesh>();
			
			
			// initialize our grid with this plane and an empty hex cell 'prefab'
			grid.initialize(plane,hex);
			// create the grid to populate cell metadata
			grid.createGrid();
			
			// an astar pathfinder to test
			var finder = new WARPathAStar();
			
			// the source of the source map
			int cellId = 3;
			
			// compute the map relative to our source
			var map = finder.getCostMap(cellId, grid);
			
			// make sure the source has no cost
			Assert.AreEqual(0, map[cellId]);
			// check a few known values at each weight
			Assert.AreEqual(1, map[2]);
			Assert.AreEqual(2, map[10]);
			Assert.AreEqual(3, map[11]);
			Assert.AreEqual(4, map[19]);
			Assert.AreEqual(5, map[20]);
		}
		
	}
}
